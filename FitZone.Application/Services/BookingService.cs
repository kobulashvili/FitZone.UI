
using AutoMapper;
using FitZone.Application.DTOs.Bookings;
using FitZone.Application.Interfaces.Services;
using FitZone.Domain.Enums;
using FitZone.Domain.Interfaces;
using FitZone.Domain.Models.Entities;

namespace FitZone.Application.Services;

public class BookingService : IBookingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BookingService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BookingResponseDto> BookAsync(
        CreateBookingDto dto)
    {
        var member = await _unitOfWork.Members
            .GetByIdAsync(dto.MemberId);

        if (member is null)
        {
            throw new InvalidOperationException("Member not found.");
        }

        var session = await _unitOfWork.WorkoutSessions
            .GetByIdWithBookingsAsync(dto.WorkoutSessionId);

        if (session is null)
        {
            throw new InvalidOperationException(
                "Workout session not found.");
        }

        if (session.IsCancelled)
        {
            throw new InvalidOperationException(
                "This workout session is cancelled.");
        }

        if (session.StartTime <= DateTime.Now)
        {
            throw new InvalidOperationException(
                "Cannot book a session that has already started.");
        }

        bool alreadyBooked = await _unitOfWork.Bookings
            .HasActiveBookingAsync(dto.MemberId, dto.WorkoutSessionId);

        if (alreadyBooked)
        {
            throw new InvalidOperationException(
                "Member already has a booking for this session.");
        }

        var memberships = await _unitOfWork.Memberships
            .GetByMemberIdAsync(dto.MemberId);

        var activeMembership = memberships.FirstOrDefault(m =>
            m.Status == MembershipStatus.Active &&
            m.EndDate >= DateTime.Now &&
            m.RemainingVisits > 0);

        if (activeMembership is null)
        {
            throw new InvalidOperationException(
                "Member does not have a valid membership.");
        }

        if (!activeMembership.CanBook(session))
        {
            throw new InvalidOperationException(
                "Membership does not allow booking this session.");
        }

        if (session.AvailablePlaces <= 0)
        {
            throw new InvalidOperationException(
                "Workout session is full.");
        }

        // Check overlapping bookings.

        var memberBookings = await _unitOfWork.Bookings
            .GetByMemberIdAsync(dto.MemberId);

        DateTime newSessionEnd =
            session.StartTime.AddMinutes(session.DurationMinutes);

        bool hasOverlappingBooking = false;

        foreach (var existingBooking in memberBookings.Where(b =>
            b.Status == BookingStatus.Confirmed &&
            b.WorkoutSessionId != session.Id))
        {
            var existingSession = await _unitOfWork.WorkoutSessions
                .GetByIdAsync(existingBooking.WorkoutSessionId);

            if (existingSession is null || existingSession.IsCancelled)
            {
                continue;
            }

            DateTime existingSessionEnd =
                existingSession.StartTime.AddMinutes(
                    existingSession.DurationMinutes);

            if (session.StartTime < existingSessionEnd &&
                newSessionEnd > existingSession.StartTime)
            {
                hasOverlappingBooking = true;
                break;
            }
        }

        if (hasOverlappingBooking)
        {
            throw new InvalidOperationException(
                "Member already has another booking at this time.");
        }

        var booking = new Booking(dto.MemberId, session.Id);

        session.AddMember(booking);

        await _unitOfWork.Bookings.AddAsync(booking);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<BookingResponseDto>(booking);
    }

    public async Task CancelAsync(int bookingId)
    {
        var booking = await _unitOfWork.Bookings
            .GetByIdAsync(bookingId);

        if (booking is null)
        {
            throw new InvalidOperationException("Booking not found.");
        }

        if (booking.Status == BookingStatus.Cancelled)
        {
            throw new InvalidOperationException(
                "Booking is already cancelled.");
        }

        var session = await _unitOfWork.WorkoutSessions
            .GetByIdAsync(booking.WorkoutSessionId);

        if (session is null)
        {
            throw new InvalidOperationException(
                "Workout session not found.");
        }

        if (session.IsCancelled)
        {
            throw new InvalidOperationException(
                "Workout session is already cancelled.");
        }

        if (session.StartTime <= DateTime.Now)
        {
            throw new InvalidOperationException(
                "Cannot cancel a booking after the session has started.");
        }

        booking.Cancel();

        await _unitOfWork.Bookings.UpdateAsync(booking);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<List<BookingResponseDto>> GetByMemberIdAsync(
        int memberId)
    {
        var bookings = await _unitOfWork.Bookings
            .GetByMemberIdAsync(memberId);

        return _mapper.Map<List<BookingResponseDto>>(bookings);
    }

    public async Task<List<BookingResponseDto>> GetBySessionIdAsync(
        int sessionId)
    {
        var bookings = await _unitOfWork.Bookings
            .GetBySessionIdAsync(sessionId);

        return _mapper.Map<List<BookingResponseDto>>(bookings);
    }
}
