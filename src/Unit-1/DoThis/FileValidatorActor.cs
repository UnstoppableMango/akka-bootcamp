using System.IO;
using Akka.Actor;

namespace WinTail
{
    public class FileValidatorActor : UntypedActor
    {
        private readonly IActorRef _consoleWriterActor;
        private readonly IActorRef _tailCoordinatorActor;

        public FileValidatorActor(IActorRef consoleWriterActor, IActorRef tailCoordinatorActor)
        {
            _consoleWriterActor = consoleWriterActor;
            _tailCoordinatorActor = tailCoordinatorActor;
        }

        protected override void OnReceive(object message)
        {
            var text = message as string;
            if (string.IsNullOrEmpty(text))
            {
                _consoleWriterActor.Tell(new NullInputError("Input was blank. Please try again.\n"));
                Sender.Tell(new ContinueProcessing());
            }
            else
            {
                var valid = IsFileUri(text);
                if (valid)
                {
                    _consoleWriterActor.Tell(new InputSuccess($"Starting processing for {text}"));
                    _tailCoordinatorActor.Tell(new TailCoordinatorActor.StartTail(text, _consoleWriterActor));
                }
                else
                {
                    _consoleWriterActor.Tell(new ValidationError($"{text} is not an existing URI on disk."));
                    Sender.Tell(new ContinueProcessing());
                }
            }
        }

        private static bool IsFileUri(string path)
        {
            return File.Exists(path);
        }
    }
}
