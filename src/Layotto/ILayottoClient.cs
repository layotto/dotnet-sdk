using System;
using System.Collections.Generic;
using Layotto.Configuration;
using Layotto.Protocol;
using Layotto.State;
using BulkStateItem = Layotto.State.BulkStateItem;
using ConfigurationItem = Layotto.Configuration.ConfigurationItem;
using PublishEventRequest = Layotto.Pubsub.PublishEventRequest;
using SaveConfigurationRequest = Layotto.Configuration.SaveConfigurationRequest;
using SayHelloRequest = Layotto.Hello.SayHelloRequest;
using SayHelloResponse = Layotto.Hello.SayHelloResponse;
using StateItem = Layotto.State.StateItem;
using StateOptions = Layotto.State.StateOptions;

namespace Layotto
{
    public interface ILayottoClient
    {
        SayHelloResponse SayHello(SayHelloRequest request);

        /// <summary>
        /// invokes service without raw data
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="methodName"></param>
        /// <param name="verb"></param>
        /// <returns></returns>
        ReadOnlyMemory<byte> InvokeMethod(string appId, string methodName, string verb);

        /// <summary>
        /// invokes service with content
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="methodName"></param>
        /// <param name="verb"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        ReadOnlyMemory<byte> InvokeMethodWithContent(string appId, string methodName, string verb,
            Invoke.DataContent content);

        /// <summary>
        /// invokes app with custom content (struct + content type).
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="methodName"></param>
        /// <param name="verb"></param>
        /// <param name="contentType"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        ReadOnlyMemory<byte> InvokeMethodWithCustomContent(string appId, string methodName, string verb,
            string contentType, object content);

        List<ConfigurationItem> GetConfiguration(ConfigurationRequestItem item);

        /// <summary>
        /// saves configuration into configuration store.
        /// </summary>
        /// <param name="request"></param>
        void SaveConfiguration(SaveConfigurationRequest request);

        /// <summary>
        /// deletes configuration from configuration store.
        /// </summary>
        /// <param name="request"></param>
        void DeleteConfiguration(ConfigurationRequestItem request);

        /// <summary>
        /// TODO gets configuration from configuration store and subscribe the updates.
        /// </summary>
        /// <param name="request"></param>
        void SubscribeConfiguration(ConfigurationRequestItem request);

        /// <summary>
        /// publishes events to the specific topic.
        /// </summary>
        /// <param name="request"></param>
        void PublishEvent(PublishEventRequest request);
        
        /// <summary>
        /// publishes data onto topic in specific pubsub component.
        /// </summary>
        /// <param name="pubsubName"></param>
        /// <param name="topicName"></param>
        /// <param name="data"></param>
        void PublishEvent(string pubsubName,string topicName,Memory<byte> data);
        
        /// <summary>
        /// serializes an object and publishes its contents as data (JSON) onto topic in specific pubsub component.
        /// </summary>
        /// <param name="pubsubName"></param>
        /// <param name="topicName"></param>
        /// <param name="data"></param>
        void PublishEventFromCustomContent(string pubsubName,string topicName,object data);
        

        /// <summary>
        /// provides way to execute multiple operations on a specified store.
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="meta"></param>
        /// <param name="ops"></param>
        void ExecuteStateTransaction(string storeName, Dictionary<string, string> meta, List<StateOperation> ops);

        /// <summary>
        /// saves the raw data into store, default options: strong, last-write
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="options"></param>
        void SaveState(string storeName, string key, Memory<byte> data, StateOptions options);

        /// <summary>
        /// saves the multiple state item to store.
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="items"></param>
        void SaveBulkState(string storeName, List<SetStateItem> items);

        /// <summary>
        /// retrieves state for multiple keys from specific store.
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="keys"></param>
        /// <param name="meta"></param>
        /// <param name="parallelism"></param>
        /// <returns></returns>
        List<BulkStateItem> GetBulkState(string storeName, List<string> keys, Dictionary<string, string> meta,
            int parallelism);

        /// <summary>
        /// retrieves state from specific store using default consistency option.
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        StateItem GetState(string storeName, string key);

        /// <summary>
        /// retrieves state from specific store using provided state consistency.
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="key"></param>
        /// <param name="meta"></param>
        /// <param name="sc"></param>
        /// <returns></returns>
        StateItem GetStateWithConsistency(string storeName, string key, Dictionary<string, string> meta,
            StateConsistency sc);

        /// <summary>
        /// deletes content from store using default state options.
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="key"></param>
        void DeleteState(string storeName, string key);

        /// <summary>
        /// deletes content from store using provided state options and etag.
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="key"></param>
        /// <param name="eTag"></param>
        /// <param name="meta"></param>
        /// <param name="opts"></param>
        void DeleteStateWithETag(string storeName, string key, ETag eTag, Dictionary<string, string> meta,
            StateOptions opts);

        /// <summary>
        /// deletes content for multiple keys from store.
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="keys"></param>
        void DeleteBulkState(string storeName, List<string> keys);

        /// <summary>
        /// deletes content for multiple keys from store.
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="items"></param>
        void DeleteBulkStateItems(string storeName, List<DeleteStateItem> items);

        TryLockResponse TryLock(TryLockRequest request);

        UnlockResponse UnLock(UnlockRequest request);

        /// <summary>
        /// Get next unique id with some auto-increment guarantee
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        GetNextIdResponse GetNextId(GetNextIdRequest request);

        /// <summary>
        /// cleans up all resources created by the client.
        /// </summary>
        void Close();
    }
}