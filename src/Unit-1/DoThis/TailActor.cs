using System.IO;
using System.Text;
using Akka.Actor;

namespace WinTail
{
    public class TailActor : UntypedActor
    {
        public record FileWrite(string FileName);

        public record FileError(string FileName, string Reason);

        public record InitialRead(string FileName, string Text);

        private readonly string _filePath;
        private readonly IActorRef _reporter;
        private readonly FileObserver _observer;
        private readonly Stream _fileStream;
        private readonly StreamReader _fileStreamReader;

        public TailActor(string filePath, IActorRef reporter)
        {
            _filePath = filePath;
            _reporter = reporter;

            _observer = new FileObserver(Self, Path.GetFullPath(_filePath));
            _observer.Start();
            
            _fileStream = new FileStream(
                Path.GetFullPath(_filePath),
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite);
            _fileStreamReader = new StreamReader(_filePath, Encoding.UTF8);

            var text = _fileStreamReader.ReadToEnd();
            Self.Tell(new InitialRead(_filePath, text));
        }

        protected override void OnReceive(object message)
        {
            if (message is FileWrite)
            {
                var text = _fileStreamReader.ReadToEnd();
                if (!string.IsNullOrEmpty(text))
                {
                    _reporter.Tell(text);
                }
            }
            else if (message is FileError error)
            {
                _reporter.Tell($"Tail error: {error.Reason}");
            }
            else if (message is InitialRead read)
            {
                _reporter.Tell(read.Text);
            }
        }
    }
}
