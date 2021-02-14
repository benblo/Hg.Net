namespace Mercurial.Net
{
    interface ICommandAwaredOfClient
    {
        /// <summary>
        /// Flag of client's type, which will be used to execute this command.
        /// </summary>
        bool UseInPersistentClient { get; set; }
    }
}
