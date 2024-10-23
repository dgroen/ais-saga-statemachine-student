using Events.SendEmailEvents;
using Events.StudentEvents;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaStateMachine
{
    public class StudentStateMachine : MassTransitStateMachine<StudentStateData>
    {
        // 4 states are going to happen
        public State AddStudent { get; private set; }
        public State CancelStudent { get; private set; }
        public State CancelSendEmail { get; private set; }
        public State SendEmail { get; private set; }

        // 4 events are going to happen

        public Event<IAddStudentEvent> AddStudentEvent { get; private set; }
        public Event<ICancelRegisterStudentEvent> CancelRegisterStudentEvent { get; private set; }
        public Event<ICancelSendEmailEvent> CancelSendEmailEvent { get; private set; }
        public Event<ISendEmailEvent> SendEmailEvent { get; private set; }

        public StudentStateMachine()
        {
            InstanceState(s => s.CurrentState);
            Event(() => AddStudentEvent, a => a.CorrelateById(m => m.Message.StudentId));
            Event(() => CancelRegisterStudentEvent, a => a.CorrelateById(m => m.Message.StudentId));
            Event(() => CancelSendEmailEvent, a => a.CorrelateById(m => m.Message.StudentId));
            Event(() => SendEmailEvent, a => a.CorrelateById(m => m.Message.StudentId));

            // A message comming from student service 
            // it could be the initially state
            Initially(
                When(AddStudentEvent).Then(context =>
                {
                    context.Saga.StudentId = context.Message.StudentId;
                    context.Saga.Title = context.Message.Title;
                    context.Saga.Email = context.Message.Email;
                    context.Saga.StudentNumber = context.Message.StudentNumber;
                    context.Saga.Age = context.Message.Age;
                    context.Saga.Location = context.Message.Location;
                }).TransitionTo(AddStudent).Publish(context => new RegisterStudentEvent(context.Saga)));

            // During AddStudentEvent some other events might occured 
            During(AddStudent, 
                When(SendEmailEvent)
                .Then(context =>
                {
                    // These values could be different 
                    context.Saga.StudentId = context.Message.StudentId;
                    context.Saga.Title = context.Message.Title;
                    context.Saga.Email = context.Message.Email;
                    context.Saga.StudentNumber = context.Message.StudentNumber;
                    context.Saga.Age = context.Message.Age;
                    context.Saga.Location = context.Message.Location;
                }).TransitionTo(SendEmail));

            During(AddStudent,
                When(CancelRegisterStudentEvent)
                .Then(context =>
                {
                    // These values could be different 
                    context.Saga.StudentId = context.Message.StudentId;
                    context.Saga.Title = context.Message.Title;
                    context.Saga.Email = context.Message.Email;
                    context.Saga.StudentNumber = context.Message.StudentNumber;
                    context.Saga.Age = context.Message.Age;
                    context.Saga.Location = context.Message.Location;
                }).TransitionTo(CancelStudent));

            // During SendEmailEvent some other events might occured 
            During(SendEmail,
                When(CancelSendEmailEvent)
                .Then(context =>
                {
                    // These values could be different 
                    context.Saga.StudentId = context.Message.StudentId;
                    context.Saga.Title = context.Message.Title;
                    context.Saga.Email = context.Message.Email;
                    context.Saga.StudentNumber = context.Message.StudentNumber;
                    context.Saga.Age = context.Message.Age;
                    context.Saga.Location = context.Message.Location;
                }).TransitionTo(CancelSendEmail));
        }

    }
}
