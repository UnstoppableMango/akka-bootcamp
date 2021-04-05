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

            // time to make your first actors!
            var consoleWriter = MyActorSystem.ActorOf(Props.Create(() => new ConsoleWriterActor()));
            var consoleReader = MyActorSystem.ActorOf(Props.Create(() => new ConsoleReaderActor(consoleWriter)));


            // tell console reader to begin
            consoleReader.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.WhenTerminated.Wait();
        }
    }
    #endregion
}
