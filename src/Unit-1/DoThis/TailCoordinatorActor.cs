using System;
using Akka.Actor;

namespace WinTail
{
    public class TailCoordinatorActor : UntypedActor
    {
        public record StartTail(string FilePath, IActorRef Reporter);

        public record StopTail(string FilePath);
        
        protected override void OnReceive(object message)
        {
            if (message is StartTail startTail)
            {
                Context.ActorOf(Props.Create(
                    () => new TailActor(startTail.FilePath, startTail.Reporter)));
            }
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                10,
                TimeSpan.FromSeconds(30),
                x => x switch {
                    ArithmeticException => Directive.Resume,
                    NotSupportedException => Directive.Stop,
                    _ => Directive.Restart,
                });
        }
    }
}
