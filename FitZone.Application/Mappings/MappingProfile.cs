
using AutoMapper;
using FitZone.Domain.Models.Entities;
using FitZone.Application.DTOs.Members;
using FitZone.Application.DTOs.Memberships;
using FitZone.Application.DTOs.Trainers;
using FitZone.Application.DTOs.WorkoutSessions;
using FitZone.Application.DTOs.Bookings;
using FitZone.Application.DTOs.Visits;

namespace FitZone.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Member
        CreateMap<Member, MemberResponseDto>();

        // Trainer
        CreateMap<Trainer, TrainerResponseDto>();

        // Membership
        CreateMap<Membership, MembershipResponseDto>()
            .ForMember(
                dest => dest.Price,
                opt => opt.MapFrom(src => src.GetPrice())
            );

        // WorkoutSession
        CreateMap<WorkoutSession, WorkoutSessionResponseDto>()
            .ForMember(
                dest => dest.OccupiedPlaces,
                opt => opt.MapFrom(src => src.OccupiedPlaces)
            )
            .ForMember(
                dest => dest.AvailablePlaces,
                opt => opt.MapFrom(src => src.AvailablePlaces)
            );

        // Booking
        CreateMap<Booking, BookingResponseDto>();

        // Visit
        CreateMap<Visit, VisitResponseDto>();
    }
}
