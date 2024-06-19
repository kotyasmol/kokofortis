using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Objects;
using Lextm.SharpSnmpLib.Pipeline;
using Lextm.SharpSnmpLib.Security;
using TFortisDeviceManager.Database;
using TFortisDeviceManager.Models;

namespace TFortisDeviceManager.Services
{
    public class SnmpTrapDaemon
    {
        static SnmpEngine? engine;
        private readonly BlockingCollection<EventModel> queueEvents;
        private readonly CancellationToken ct;

        public SnmpTrapDaemon(BlockingCollection<EventModel> queueEvents, CancellationToken ct)
        {
            this.queueEvents = queueEvents;
            this.ct = ct;
        }

        public void Start()
        {
            var store = new ObjectStore();
            store.Add(new SysDescr());
            store.Add(new SysObjectId());
            store.Add(new SysUpTime());
            store.Add(new SysContact());
            store.Add(new SysName());
            store.Add(new SysLocation());
            store.Add(new SysServices());
            store.Add(new SysORLastChange());
            store.Add(new SysORTable());
            store.Add(new IfNumber());
            store.Add(new IfTable());

            var users = new UserRegistry();
            users.Add(new OctetString("neither"), DefaultPrivacyProvider.DefaultPair);
            users.Add(new OctetString("authen"), new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("authentication"))));


            if (DESPrivacyProvider.IsSupported)
            {
                users.Add(new OctetString("privacy"), new DESPrivacyProvider(new OctetString("privacyphrase"),
                                                                            new MD5AuthenticationProvider(new OctetString("authentication"))));
            }

            if (AESPrivacyProviderBase.IsSupported)
            {
                users.Add(new OctetString("aes"), new AESPrivacyProvider(new OctetString("privacyphrase"), new MD5AuthenticationProvider(new OctetString("authentication"))));
                users.Add(new OctetString("aes192"), new AES192PrivacyProvider(new OctetString("privacyphrase"), new MD5AuthenticationProvider(new OctetString("authentication"))));
                users.Add(new OctetString("aes256"), new AES256PrivacyProvider(new OctetString("privacyphrase"), new MD5AuthenticationProvider(new OctetString("authentication"))));
            }

            var getv1 = new GetV1MessageHandler();
            var getv1Mapping = new HandlerMapping("v1", "GET", getv1);


            var v1 = new Version1MembershipProvider(new OctetString("public"), new OctetString("public"));
            var membership = new ComposedMembershipProvider(new IMembershipProvider[] { v1 });
            var handlerFactory = new MessageHandlerFactory(new[] { getv1Mapping });
            var pipelineFactory = new SnmpApplicationFactory(store, membership, handlerFactory);


            engine = new SnmpEngine(pipelineFactory, new Listener { Users = users }, new EngineGroup());

            engine.Listener.AddBinding(new IPEndPoint(IPAddress.Any, 162));
            engine.Listener.MessageReceived += RequestReceived;
            engine?.Start();

        }

        public void Stop()
        {
            engine?.Stop();
            engine?.Dispose();
        }

        private void RequestReceived(object? sender, MessageReceivedEventArgs? e)
        {
            var oid = e.Message.Variables()[0].Id;
            var data = e.Message.Variables()[0].Data;
            string ip = e.Sender.Address.ToString();
            string community = e.Message.Community().ToString();

            var communityFromDb = PGDataAccess.GetCommunity(ip);
            if (communityFromDb == null || communityFromDb != community)
                return;

            // Получаем название сенсора
            var sensorName = PGDataAccess.GetSensorName(oid.ToString());
            if (sensorName == null)
                return;

            var sensor = PGDataAccess.GetSensor(ip, sensorName);
            if (sensor == null)
                return;

            var deviceLocation = PGDataAccess.GetDeviceLocation(ip);
            var deviceDescription = PGDataAccess.GetDeviceDescription(ip);

            string statusOk = Properties.Resources.StatusOk;
            string statusProblem = Properties.Resources.StatusProblem;

            string? sensorValueText = "unknown";
            string status = statusProblem;

            if (sensor.Enable)
            {
                Console.WriteLine($"Trap was receive Data: {data}");
                Dictionary<int, string?> okValue = PGDataAccess.GetOkValueDict(oid.ToString());
                Dictionary<int, string?> badValue = PGDataAccess.GetBadValueDict(oid.ToString());
                string? description = PGDataAccess.GetDescription(oid.ToString());
                int sensorValue = int.Parse(data.ToString());

                if (okValue.ContainsKey(sensorValue))
                {
                    sensorValueText = okValue[sensorValue];
                    if (!sensor.Invert)
                    {
                        status = statusOk;
                    }
                    else
                    {
                        status = statusProblem;
                    }
                }
                else if (badValue.ContainsKey(sensorValue))
                {
                    sensorValueText = badValue[sensorValue];
                    if (!sensor.Invert)
                    {
                        status = statusProblem;
                    }
                    else
                    {
                        status = statusOk;
                    }
                }

                bool fieldsNotNull = (sensorValueText != null) && (description != null) && (status != null);
                if (fieldsNotNull)
                {
                    EventModel evnt = MonitoringEventService.CreateEvent(sensor.DeviceName, sensor.Ip, deviceDescription, deviceLocation, sensorName, sensorValueText, description, status);

                    try
                    {
                        queueEvents.TryAdd(evnt, 2, ct);
                    }
                    catch (OperationCanceledException)
                    {
                        //Stop
                    }
                }
            }
        }
    }
}
