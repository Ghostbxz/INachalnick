using Elastic.Apm.MongoDb;
using MongoDB.Driver.Core.Events;
using System;

namespace INachalnicUtilities.DataAccess.Mongo
{
    public class SafeMongoDbEventSubscriber : IEventSubscriber
    {
        private MongoDbEventSubscriber mongoDbEventSubscriber = new MongoDbEventSubscriber();
        public bool TryGetEventHandler<TEvent>(out Action<TEvent> handler)
        {
            if (mongoDbEventSubscriber.TryGetEventHandler<TEvent>(out var action))
            {
                handler = x =>
                {
                    try
                    {
                        action(x);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"SafeMongoDbEventSubscriber - {ex}");
                    }
                };
                return true;
            }
            handler = x => { };
            return false;
        }
    }
}