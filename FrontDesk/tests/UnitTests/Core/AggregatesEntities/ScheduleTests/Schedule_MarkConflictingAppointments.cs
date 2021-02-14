﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using FrontDesk.Core.Aggregates;
using PluralsightDdd.SharedKernel;
using Xunit;

namespace UnitTests.Core.AggregatesEntities.ScheduleTests
{
  public class Schedule_MarkConflictingAppointments
  {
    private readonly Guid _scheduleId = Guid.Parse("4a17e702-c20e-4b87-b95b-f915c5a794f7");
    private readonly DateTime _startTime = new DateTime(2021, 01, 01, 10, 00, 00);
    private readonly DateTime _endTime = new DateTime(2021, 01, 01, 11, 00, 00);
    private readonly DateTimeRange _dateRange;
    private readonly int _clinicId = 1;

    public Schedule_MarkConflictingAppointments()
    {
      _dateRange = new DateTimeRange(_startTime, _endTime);
    }

    [Fact]
    public async Task SetsProperties()
    {
      // verify this method
      //     public Schedule(Guid id, DateTimeRange dateRange, int clinicId, IEnumerable<Appointment> appointments)

      var appointments = new List<Appointment>();

      var schedule = new Schedule(_scheduleId, _dateRange, _clinicId, appointments);

      Assert.Equal(_scheduleId, schedule.Id);
      Assert.Equal(_startTime, schedule.DateRange.Start);
      Assert.Equal(_endTime, schedule.DateRange.End);
      Assert.Equal(_clinicId, schedule.ClinicId);
      Assert.Equal(appointments, schedule.Appointments);
    }

    [Fact]
    public async Task MarksConflictingAppointments()
    {
      // given appts with conflicts verify it marks them

      var schedule = new Schedule(_scheduleId, _dateRange, _clinicId, null);
      var appointmentType = 1;
      var doctorId = 2;
      var patientId = 3;
      var roomId = 4;

      var lisaTitle = "Lisa Appointment";
      var lisaAppointment = new Appointment(appointmentType, _scheduleId, _clinicId, doctorId, patientId, roomId, _dateRange, lisaTitle);
      schedule.AddNewAppointment(lisaAppointment);

      var mimiTitle = "Mimi Appointment";
      var mimiAppointment = new Appointment(appointmentType, _scheduleId, _clinicId, doctorId, patientId, roomId, _dateRange, mimiTitle);
      schedule.AddNewAppointment(mimiAppointment);

      Assert.True(lisaAppointment.IsPotentiallyConflicting);
      Assert.True(mimiAppointment.IsPotentiallyConflicting);
    }
  }
}
