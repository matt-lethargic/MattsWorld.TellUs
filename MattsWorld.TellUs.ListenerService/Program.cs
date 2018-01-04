using Topshelf;

namespace MattsWorld.TellUs.ListenerService
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<TellUsListenerService>(s =>
                {
                    s.ConstructUsing(name => new TellUsListenerService());
                    s.WhenStarted(y => y.Start());
                    s.WhenStopped(y => y.Stop());
                });

                x.EnableServiceRecovery(s =>
                {
                    s.RestartService(1);
                });

                x.RunAsLocalSystem();
                x.SetDescription("Tell Us Listener");
                x.SetDisplayName("TellUs Listner");
                x.SetServiceName("TellUsListener");
            });
        }
    }
}
