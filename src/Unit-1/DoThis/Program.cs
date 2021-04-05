using System;
using Akka.Actor;

namespace WinTail
{
    #region Program
    class Program
    {
        public static ActorSystem MyActorSystem;

        static void Main(string[] args)
        {
            // initialize MyActorSystem
            MyActorSystem = ActorSystem.Create("MyActorSystem");

            var consoleWriterProps = Props.Create<ConsoleWriterActor>();
            var consoleWriter = MyActorSystem.ActorOf(consoleWriterProps, "consoleWriterActor");
            
            var validationProps = Props.Create(() => new ValidationActor(consoleWriter));
            var validation = MyActorSystem.ActorOf(validationProps, "validationActor");
            
            var consoleReaderProps = Props.Create<ConsoleReaderActor>(validation);
            var consoleReader = MyActorSystem.ActorOf(consoleReaderProps, "consoleReaderActor");

            // tell console reader to begin
            consoleReader.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.WhenTerminated.Wait();
        }
    }
    #endregion
}
