using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TFortisDeviceManager.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using System.Globalization;

namespace TFortisDeviceManager.Database
{
    public static class PGDataAccess
    {
        public static string GetDeviceName(int deviceId)
        {
            using TfortisdbContext database = new();
            var device = database.DeviceTypes.Where(t=> t.Id == deviceId).FirstOrDefault();
                        
            return device?.Name ?? "Unknown Device";      
        }

        public static List<string?> GetAllDeviceModels()
        {
            using TfortisdbContext database = new();
            var models = database.DeviceTypes.Select(t => t.Name).ToList();

            return models;
        }

        public static int GetPortsCount(int deviceId)
        {
            using TfortisdbContext database = new();
            var entity = database.DeviceTypes.Where(t => t.Id == deviceId).FirstOrDefault();
            return entity?.PortsPoe + entity?.PortsSfp + entity?.PortsWithoutPoe ?? 0;
        }

        public static int GetPortsUplinkCount(int deviceId)
        {
            using TfortisdbContext database = new();
            var entity = database.DeviceTypes.Where(t=>t.Id == deviceId).FirstOrDefault();
            return entity?.PortsUplink ?? 0;
        }

        public static string? GetDeviceLocation(string deviceIp)
        {
            using TfortisdbContext database = new();
            var entity = database.DeviceForMonitoring.Where(t => t.Ip == deviceIp).FirstOrDefault();
            return entity?.Location;
        }

        public static string? GetDeviceDescription(string deviceIp)
        {
            using TfortisdbContext database = new();
            var entity = database.DeviceForMonitoring.Where(t => t.Ip == deviceIp).FirstOrDefault();
            return entity?.Description;
        }

        public static int GetDeviceTypeId(string deviceName)
        {
            using TfortisdbContext database = new();
            var entity = database.DeviceTypes.Where(t => t.Name == deviceName).FirstOrDefault();

            return entity?.Id ?? 0;
        }

        public static int GetUptime(MonitoringDevice device)
        {
            using TfortisdbContext database = new();
            var entity = database.DeviceForMonitoring.Where(t => t.Ip == device.IpAddress && t.Mac == device.Mac).FirstOrDefault();
            return entity?.Uptime ?? 0;
        }
        
        public static void SetUptime(MonitoringDevice device)
        {
            using TfortisdbContext database = new();
            var entity = database.DeviceForMonitoring.Where(t => t.Ip == device.IpAddress && t.Mac == device.Mac).FirstOrDefault();

            if (entity == null) return;

            entity.Uptime = device.Uptime;

            database.DeviceForMonitoring.Update(entity);
            database.SaveChanges();
        }
        
        public static List<OidsForDevice> GetOids(uint deviceId)
        {
            using TfortisdbContext database = new();
            var oidsForDevice = database.OidsForDevices.Where(t => t.DeviceTypeId == deviceId).ToList();
            return oidsForDevice;
        }
        
        public static int? GetOid(int deviceId, string oidName)
        {
            using TfortisdbContext database = new();
            var entity = database.OidsForDevices.Where(t => t.DeviceTypeId == deviceId && t.Name == oidName).FirstOrDefault();
            return entity?.Key;
        }
        
        public static List<string?> GetUniqueNamesOid()
        {
            using TfortisdbContext database = new();
            var result = database.OidsForDevices.Select(t => t.Name).Distinct().ToList();
            return result;
        }
        
        public static string? GetDescriptionsOid(string name)
        {
            using TfortisdbContext database = new();
            var entity = database.OidsForDevices.Where(t => t.Name == name).FirstOrDefault();
            return entity?.Description;
        }
        
        public static bool GetInvertible(int deviceId)
        {
            using TfortisdbContext database = new();
            var entity = database.OidsForDevices.Where(t => t.DeviceTypeId == deviceId).FirstOrDefault();

            if (entity == null) return false;

            return Convert.ToBoolean(entity.Invertible);
        }
        
        public static bool CheckIfDeviceExists(string? Ip)
        {
            using TfortisdbContext database = new();
            var device = database.DeviceForMonitoring.Where(t => t.Ip == Ip).FirstOrDefault();

            return device != null;         
        }

        public static int? GetKeyFromOidsForDevice(int id, string name)
        {
            using TfortisdbContext database = new();
            var entity = database.OidsForDevices.Where(t => t.DeviceTypeId == id && t.Name == name).FirstOrDefault();

            return entity?.Key ?? null;          
        }
        
        public static bool? GetSensorEnable(int oidForDeviceKey)
        {
            using TfortisdbContext database = new();
            var entity = database.DeviceAndOids.Where(t => t.OidForDeviceKey == oidForDeviceKey).FirstOrDefault();

            if (entity == null) return false;

            return Convert.ToBoolean(entity.Enable);
        }

        public static Sensor GetSensor(string ip, string sensorName)
        {
            using TfortisdbContext database = new();

            var devTypeId = database.DeviceForMonitoring.Where(t => t.Ip == ip).Select(t => t.DeviceTypeId).FirstOrDefault();

            var oidKey = database.OidsForDevices.Where(t => t.DeviceTypeId == devTypeId && t.Name == sensorName).FirstOrDefault();

            var sensorFromDb = (from devAndOid in database.DeviceAndOids
                             join dev in database.DeviceForMonitoring on devAndOid.Key equals dev.Key
                             join type in database.DeviceTypes on dev.DeviceTypeId equals type.Id

                             select new
                             {
                                 dev.Ip,
                                 type.Name,
                                 devAndOid.Invert,
                                 devAndOid.Enable

                             }).FirstOrDefault();
            Sensor sensor = new()
            {
                Ip = sensorFromDb.Ip,
                Name = sensorFromDb.Name,
                Invert = sensorFromDb.Invert,
                Enable = sensorFromDb.Enable
            };
                 
            return sensor;
        }

        public static bool GetSensorInvert(string ip, string deviceName, string sensorName)
        {
            using TfortisdbContext database = new();

            var deviceKey = database.DeviceForMonitoring.FirstOrDefault(x => x.Ip == ip).Key;

            var deviceId = database.DeviceTypes.FirstOrDefault(x => x.Name == deviceName).Id;

            var oidKey = database.OidsForDevices.FirstOrDefault(x => x.DeviceTypeId == deviceId && x.Name == sensorName).Key;

            var invert = database.DeviceAndOids.OrderByDescending(x => x.Key).FirstOrDefault(x => x.DeviceIp == ip && x.OidForDeviceKey == oidKey && x.DeviceForMonitoringKey == deviceId).Invert;

            return invert;
        }

        public static string GetDeviceHostStatus(string ip, string name)
        {
            using TfortisdbContext database = new();
            
            var hostStatus = database.Events.Where(t => t.DeviceName == name && t.Ip == ip && t.SensorName == Properties.Resources.HostStatus && t.Status != "Info" && t.Status != "OLD").Select(t => t.Description).FirstOrDefault();

            return hostStatus ?? "";
        }

        public static List<MonitoringDevice> GetDevicesForMonitoring()
        { 
            using TfortisdbContext database = new();
            var devices = from dev in database.DeviceForMonitoring
                          join type in database.DeviceTypes on dev.DeviceTypeId equals type.Id
                          select new 
                          { 
                              dev.Ip,
                              dev.Mac,
                              type.PortsSfp,
                              type.PortsPoe,
                              type.PortsUplink,
                              type.PortsWithoutPoe,
                              type.Ups,
                              dev.Uptime,
                              dev.DeviceTypeId,
                              dev.Location,
                              dev.Description,
                              dev.SerialNumber,
                              type.Name,
                              
                          };

            List<MonitoringDevice> Devices = new();
            Devices.Clear();
            foreach (var d in devices)
            {
                MonitoringDevice device = new(d.Name, d.Ip, d.Mac, d.SerialNumber, d.Location, d.Description)
                {
                    Id = d.DeviceTypeId,
                    PortsPoe = (int)d.PortsPoe,
                    Uptime = d.Uptime
                };

                Devices.Add(device);
            }

            return Devices;
        }
                
        public static void AddDeviceForMonitoring(MonitoringDevice device, string community, int sendEmail)
        {
            using TfortisdbContext database = new();
            int rowCount = database.DeviceForMonitoring.Count();
            DeviceForMonitoring deviceForMonitoring = new()
            {
                DeviceTypeId = GetDeviceTypeId(device.Name),
                Ip = device.IpAddress,
                Mac = device.Mac,
                Location = device.Location,
                Description = device.Description,
                Community = community,
                SendEmail = sendEmail,
                SerialNumber = device.SerialNumber
            };

            database.DeviceForMonitoring.Add(deviceForMonitoring);
            database.SaveChanges();
        }
        
        public static string GetCommunity(string ip)
        {
            using TfortisdbContext database = new();
            var entity = database.DeviceForMonitoring.Where(t => t.Ip == ip).FirstOrDefault();

            return entity?.Community ?? "";
        }
        
        public static bool GetSendEmail(string ip)
        {
            using TfortisdbContext database = new();
            var entity = database.DeviceForMonitoring.Where(t => t.Ip == ip).FirstOrDefault();
            if (entity == null) return false;
                
            return Convert.ToBoolean(entity.SendEmail, CultureInfo.InvariantCulture);
        }
        
        public static bool GetSendEmail(string ip, string sensorName)
        {
            using TfortisdbContext database = new();

            var deviceKey = database.DeviceForMonitoring.Where(t => t.Ip == ip).Select(t => t.DeviceTypeId).FirstOrDefault();
            var oidKey = database.OidsForDevices.Where(t => t.Name == sensorName && t.DeviceTypeId == deviceKey).Select(t => t.Key).FirstOrDefault();
            var entity = database.DeviceAndOids.OrderByDescending(t => t.Key).Where(t => t.DeviceForMonitoringKey == deviceKey && t.OidForDeviceKey == oidKey).FirstOrDefault();

            if (entity == null)
                return false;
            else
                return entity.SendEmail;
        }
        
        public static void SetCommunity(string ip, string community)
        {
            using TfortisdbContext database = new();
            var entity = database.DeviceForMonitoring.Where(t => t.Ip == ip).FirstOrDefault();

            if (entity == null) return;

            entity.Community = community;
            database.DeviceForMonitoring.Update(entity);
            database.SaveChanges();
        }
        
        public static void SetSendEmail(string ip, int sendEmail)
        {
            using TfortisdbContext database = new();
            var entity = database.DeviceForMonitoring.Where(t => t.Ip == ip).FirstOrDefault();

            if (entity == null) return;

            entity.SendEmail = sendEmail;
            database.DeviceForMonitoring.Update(entity);
            database.SaveChanges();
        }
        
      
        public static void DelDevice(MonitoringDevice device)
        {
            using TfortisdbContext database = new();
            DeviceForMonitoring delDevice = new();
           
                var entity = database.DeviceForMonitoring.Where(t => t.Ip == device.IpAddress).FirstOrDefault();

                delDevice.Ip = device.IpAddress;
                delDevice.Mac = device.Mac;
                delDevice.SerialNumber = device.SerialNumber;

            if (entity == null) return;

            database.DeviceForMonitoring.Remove(entity);
            database.SaveChanges();    
        }       
        
        public static void SaveDeviceAndOids(List<DeviceAndOid> deviceAndOids)
        {
            using TfortisdbContext database = new();

            foreach(var oid in deviceAndOids)
            {
                database.DeviceAndOids.Add(oid);
            }

            database.SaveChanges();
        }
        
        public static string? GetSensorName(string oid)
        {
            using TfortisdbContext database = new();
            var entity = database.TrapOids.Where(t => t.Address == oid).FirstOrDefault();
            return entity?.Name ?? "";
        }
        
        public static string? GetDescription(string oid)
        {
            using TfortisdbContext database = new();
            var entity = database.TrapOids.Where(t => t.Address == oid).FirstOrDefault();
            return entity?.Description ?? "";
        }
        
        public static Dictionary<int, string?> GetOkValueDict(string oid)
        {
            using TfortisdbContext database = new();
            var entity = database.TrapOids.Where(t => t.Address == oid).FirstOrDefault();

            Dictionary<int, string?> pair = new(){ };

            if(entity != null)           
                pair.Add(entity.OkValue, entity.OkValueText);

            return pair;
        }
        
        public static Dictionary<int, string?> GetBadValueDict(string oid)
        {
            using TfortisdbContext database = new();
            var entity = database.TrapOids.Where(t => t.Address == oid).FirstOrDefault();

            Dictionary<int, string?> pair = new(){ };

            if (entity != null)
                pair.Add(entity.BadValue, entity.BadValueText);

            return pair;
        }
        
        public static List<Sensor> LoadOidsForMonitroing()
        {
            using TfortisdbContext database = new();
            var oids = (from oid in database.DeviceAndOids
                       join device in database.DeviceForMonitoring on oid.DeviceForMonitoringKey equals device.Key
                       join oidsForDevice in database.OidsForDevices on oid.OidForDeviceKey equals oidsForDevice.Key
                       join devType in database.DeviceTypes on device.DeviceTypeId equals devType.Id
                       orderby oidsForDevice.Address
                       select new
                       {
                           device.Key,
                           device.Ip,
                           DeviceName = devType.Name,
                           oidsForDevice.Name,
                           oidsForDevice.Address,
                           oidsForDevice.Description,
                           oidsForDevice.OkValue,
                           oidsForDevice.OkValueText,
                           oidsForDevice.BadValue,
                           oidsForDevice.BadValueText,
                           oid.Invertible,
                           oid.Invert,
                           oid.Timeout,
                           oid.Enable,
                       }).AsEnumerable();

            List<Sensor> Sensors = new();

            foreach(var o in oids)
            {
                Sensor sensor = new()
                {
                    Key = o.Key,
                    Ip = o.Ip,
                    DeviceName = o.DeviceName,
                    Description = o.Description,
                    OkValue = o.OkValue,
                    OkValueText = o.OkValueText,
                    BadValue = o.BadValue,
                    BadValueText = o.BadValueText,
                    Invertible = Convert.ToBoolean(o.Invertible),
                    Invert = Convert.ToBoolean(o.Invert),
                    Timeout = (int)o.Timeout,
                    Enable = Convert.ToBoolean(o.Enable)
                };

                Sensors.Add(sensor);
            }

            return Sensors;
        }

        public static List<Sensor> LoadOidsForMonitroing(string deviceIP)
        {
            using TfortisdbContext database = new();

            List<Sensor> Sensors = new();
            /*
                        var deviceForMonitoring = database.DeviceForMonitoring.Where(t => t.Ip == deviceIP).FirstOrDefault();

                        if (deviceForMonitoring == null) return Sensors;

                        var deviceType = database.DeviceTypes.Where(t => deviceForMonitoring.DeviceTypeId == t.Id).FirstOrDefault();

                        var deviceAndOids = database.DeviceAndOids.Where(t => t.DeviceForMonitoringKey == deviceType.Id)
                            .ToList()
                            .OrderBy(x => x.Key)
                            .Reverse();

                        deviceAndOids = deviceAndOids.DistinctBy(t => t.OidForDeviceKey);

                        var result = deviceAndOids.Join(database.OidsForDevices, 
                            t => t.OidForDeviceKey, 
                            d => d.Key, 
                            (t, d) => new 
                            {
                                Key = t.OidForDeviceKey,
                                deviceForMonitoring.Ip,
                                d.Address,
                                d.Name,
                                d.Description,
                                d.OkValue,
                                d.OkValueText,
                                d.BadValue,
                                d.BadValueText,
                                t.Timeout,
                                t.Invertible,
                                t.Invert,
                                t.Enable,
                                t.SendEmail,
                                DeviceName = deviceType?.Name,
                            });*/

            var deviceForMonitoring = database.DeviceForMonitoring.Where(t => t.Ip == deviceIP).FirstOrDefault();

            var deviceType = database.DeviceTypes.Where(t => deviceForMonitoring.DeviceTypeId == t.Id).FirstOrDefault();

            var deviceAndOids = database.DeviceAndOids.Where(t => t.DeviceForMonitoringKey == deviceType.Id && t.DeviceIp == deviceIP).ToList().OrderBy(x => x.Key).Reverse();
            deviceAndOids = deviceAndOids.DistinctBy(t => t.OidForDeviceKey);

            var result = deviceAndOids.Join(database.OidsForDevices,
                t => t.OidForDeviceKey,
                d => d.Key,
                (t, d) => new
                {
                    Key = t.OidForDeviceKey,
                    t.DeviceIp,
                    d.Address,
                    d.Name,
                    d.Description,
                    d.OkValue,
                    d.OkValueText,
                    d.BadValue,
                    d.BadValueText,
                    t.Timeout,
                    t.Invertible,
                    t.Invert,
                    t.Enable,
                    t.SendEmail,
                    DeviceName = deviceType.Name,
                }).Where(t => t.DeviceIp == deviceIP);

            foreach (var oid in result)
            {
                Sensor sensor = new()
                {
                    Key = oid.Key,
                    Ip = oid.DeviceIp,
                    Name = oid.Name,
                    Address = oid.Address,
                    DeviceName = oid.DeviceName,
                    Description = oid.Description,
                    OkValue = oid.OkValue,
                    OkValueText = oid.OkValueText,
                    BadValue = oid.BadValue,
                    BadValueText = oid.BadValueText,
                    Invertible = Convert.ToBoolean(oid.Invertible),
                    Invert = Convert.ToBoolean(oid.Invert),
                    Timeout = (int)oid.Timeout,
                    Enable = Convert.ToBoolean(oid.Enable),
                };

                if (sensor.Ip == deviceIP)
                    Sensors.Add(sensor);
            }

            return Sensors;
        }      
        
        public static List<OidsForDevice> GetOidsForMonitoring(string deviceIP)
        {
            using TfortisdbContext database = new();
           
                        var deviceForMonitoring = database.DeviceForMonitoring.Where(t => t.Ip == deviceIP).FirstOrDefault();

                        var deviceType = database.DeviceTypes.Where(t => deviceForMonitoring.DeviceTypeId == t.Id).FirstOrDefault();

                        var deviceAndOids = database.DeviceAndOids.Where(t => t.DeviceForMonitoringKey == deviceType.Id && t.DeviceIp == deviceIP).ToList().OrderBy(x => x.Key).Reverse();
                        deviceAndOids = deviceAndOids.DistinctBy(t => t.OidForDeviceKey);

                        var rawOids = deviceAndOids.Join(database.OidsForDevices,
                            t => t.OidForDeviceKey,
                            d => d.Key,
                            (t, d) => new
                            {
                                Key = t.OidForDeviceKey,
                                t.DeviceIp, 
                                d.Address,
                                d.Name,
                                d.Description,
                                d.OkValue,
                                d.OkValueText,
                                d.BadValue,
                                d.BadValueText,
                                t.Timeout,
                                t.Invertible,
                                t.Invert,
                                t.Enable,
                                t.SendEmail,
                                DeviceName = deviceType.Name,
                            }).Where(t => t.DeviceIp == deviceIP);

                        List<OidsForDevice> result = new();

                        foreach (var oid in rawOids)
                        {
                            OidsForDevice oidForDevice = new()
                            {
                                Key = oid.Key,
                                Name = oid.Name,
                                Description = oid.Description,
                                Timeout = (int)oid.Timeout,
                                Invertible = Convert.ToInt32(oid.Invertible),                                
                                Enable = Convert.ToInt32(oid.Enable)
                            };

                            result.Add(oidForDevice);
                        }

                        return result;
        }

        public static void ClearDeviceAndOids()
        {
           // 
        }

        public static string GetLastDeviceState(string deviceIP)
        {
            using TfortisdbContext database = new();
            var device = database.Events.Where(t => t.Ip == deviceIP && t.SensorName == Properties.Resources.HostStatus && t.SensorValueText != Properties.Resources.HostStatusReloaded && t.Status != "OLD").OrderByDescending(t => t.Id).FirstOrDefault();

            return device?.SensorValueText ?? "";         
        }
        
        public static string GetSensorLastState(string deviceIP, string sensorName)
        {
            using TfortisdbContext database = new();
            var entity = database.Events.Where(t => t.Ip == deviceIP && t.SensorName == sensorName && t.Status != "OLD").OrderBy(t => t.Id).FirstOrDefault();
            return entity?.SensorValueText ?? "";
        }
        
        public static void AddEvent(EventModel evnt)
        {
            using TfortisdbContext database = new();

            Event newEvent = new()
            {
                Time = evnt.Time,
                DeviceName = evnt.DeviceName,
                Ip = evnt.Ip,
                SensorName = evnt.SensorName,
                SensorValueText = evnt.SensorValueText,
                Description = evnt.Description,
                Status = evnt.Status,
                DeviceLocation = evnt.DeviceLocation,
                DeviceDescription = evnt.DeviceDescription,
                Age = evnt.Age
            };

            database.Events.Add(newEvent);
            database.SaveChanges();
        }
        
        public static void DelEvent(string dateTime)
        {
            using TfortisdbContext database = new();
            

            var oldEvents = database.Events.ToList();

            oldEvents = oldEvents.Where(t => DateTime.Parse(t.Time, CultureInfo.InvariantCulture) < DateTime.Parse(dateTime, CultureInfo.InvariantCulture)).ToList();

            if (oldEvents != null && oldEvents.Count > 0)
            {
                foreach (var evnt in from evnt in oldEvents
                                     where DateTime.Parse(evnt.Time, CultureInfo.InvariantCulture) < DateTime.Parse(dateTime, CultureInfo.InvariantCulture)
                                     select evnt)
                {
                    database.Events.Remove(evnt);
                }

                database.SaveChanges();
            }
        }
        
        public static List<EventModel> LoadEventsFromDateToDate(DateTime fromDate, DateTime toDate)
        {
            using TfortisdbContext database = new();

            var newToDate = toDate.AddDays(2); 
            var events = database.Events.ToList();

            List<EventModel> agregatedEvents = new();

            agregatedEvents.AddRange(from evnt in events
                                     let eventDate = DateTime.ParseExact(evnt.Time, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                                     where eventDate > fromDate && eventDate < toDate
                                     select evnt.ToEventModel());
            return agregatedEvents;         
        }
        
        public static List<EventModel> LoadEventsWithoutOldAsync()
        {
            using TfortisdbContext database = new();
            var events = database.Events.Where(t => t.Status != "OLD").OrderBy(
                t => t.Status == "WARNING" ? 0 : 
                t.Status == "ERROR" ? 1 : 
                t.Status == "INFO" ? 2 : 
                t.Status == "OK" ? 3 : 
                1).ToList(); 

            List<EventModel> result = new();

            foreach(var e in events)
            {
                result.Add(e.ToEventModel());
            }

            return result;
        }

        public static List<EventModel> LoadEventsWithoutOldWithDeviceByIp(string ip)
        {
            using TfortisdbContext database = new();
            var events = database.Events.Where(t => t.Status != "OLD" &&  t.Ip == ip).OrderBy(x => x.Status).ToList();

            List<EventModel> result = new();

            foreach (var e in events)
            {
                result.Add(e.ToEventModel());
            }

            return result;
        }

        public static List<EventModel> LoadEventsWithoutOldWithDeviceAsync(MonitoringDevice device)
        {
            using TfortisdbContext database = new();
            var events = database.Events.Where(t => t.Status != "OLD" && t.DeviceName == device.Name && t.Ip == device.IpAddress).ToList();

            List<EventModel> result = new();

            foreach (var e in events)
            {
                result.Add(e.ToEventModel());
            }

            return result;
        }

        public static EventModel? SelectOneFromEvent(EventModel evnt)
        {
            using TfortisdbContext database = new();
            var entity = database.Events.Where(t => t.DeviceName == evnt.DeviceName && t.SensorName == evnt.SensorName && t.Ip == evnt.Ip && t.Status != "OLD").FirstOrDefault();

            if (entity == null) return null;

             return entity.ToEventModel();
        }
        
        public static EventModel? SelectOneEventHostStatus(EventModel evnt, string hostStatus)
        {
            using TfortisdbContext database = new();
            var entity = database.Events.Where(t => t.SensorValueText == hostStatus && 
            t.Ip == evnt.Ip && 
            t.DeviceName == evnt.DeviceName && 
            t.SensorName == evnt.SensorName && 
            t.Status != "OLD").AsEnumerable().FirstOrDefault();

            if (entity == null) return null;

            return entity.ToEventModel();
        }
      
        public static int UpdateEventAge(EventModel evnt)
        {
            using TfortisdbContext database = new();
            int updatedRows = database.Database.ExecuteSqlRaw($"UPDATE Events SET age = {evnt.Age!} WHERE deviceName = {evnt.DeviceName!} and sensorName = {evnt.SensorName!} and ip = {evnt.Ip} and status = {evnt.Status};");
            database.SaveChanges();
            return updatedRows;
        }
        
        public static void UpdateEventAge(EventModel evnt, string? description)
        {
            using TfortisdbContext database = new();
  
            var eventToUpdate = database.Events.Where(t => t.DeviceName == evnt.DeviceName && t.SensorName == evnt.SensorName && t.Ip == evnt.Ip && t.Status == evnt.Status).FirstOrDefault();

            if (eventToUpdate == null) return;
            
            eventToUpdate.Age = evnt.Age;
            eventToUpdate.Description = description;

            database.Events.Update(eventToUpdate);

            database.SaveChanges();       
        }

        public static List<string> LoadDeviceModels()
        {
            using TfortisdbContext database = new();
            var output = database.Events.Select(e => e.DeviceName).Distinct().ToList();
            return output;
        }

        public static List<string> LoadDeviceIPs()
        {
            using TfortisdbContext database = new();
            var output = database.Events.Select(e => e.Ip).Distinct().ToList();
            return output;
        }

        public static List<string> LoadEventStatus()
        {
            using TfortisdbContext database = new();
            var output = database.Events.Select(e => e.Status).Distinct().ToList();
            return output;
        }

        public static List<string> LoadEventStatusWithoutOld()
        {
            using TfortisdbContext database = new();
            var output = database.Events.Where(t => t.Status != "OLD").Select(t => t.Status).Distinct().ToList();
            return output;
        }

        public static List<string> LoadDeviceParameters()
        {
            using TfortisdbContext database = new();
            var output = database.Events.OrderBy(t => t.SensorName).Select(t => t.SensorValueText).Distinct().ToList();
            return output;
        }
        
        public static void UpdateEventStatusToOld(EventModel evnt)
        {
            using TfortisdbContext database = new();
            var entity = database.Events.Where(t => t.DeviceName == evnt.DeviceName && t.SensorName == evnt.SensorName && t.Ip == evnt.Ip && t.Status != "OLD").FirstOrDefault();

            if (entity == null) return;
            
            entity.Status = "OLD";
            database.Events.Update(entity);
            database.SaveChanges();            
        }

        public static void UpdateEventStatusToOldWithTime(EventModel evnt)
        {
            using TfortisdbContext database = new();
            var entity = database.Events.Where(t => t.Time == evnt.Time && t.DeviceName == evnt.DeviceName && t.SensorName == evnt.SensorName && t.Ip == evnt.Ip && t.Status != "OLD").FirstOrDefault();

            if (entity == null) return;

            entity.Status = "OLD";
            database.Events.Update(entity);
            database.SaveChanges();
        }

        public static void UpdateEventStatusToOld(EventModel evnt, string sensorValue)
        {
            using TfortisdbContext database = new();
            var entity = database.Events.Where(t => t.DeviceName == evnt.DeviceName && t.SensorName == evnt.SensorName && t.Ip == evnt.Ip && t.SensorValueText == sensorValue && t.Status != "OLD").FirstOrDefault();

            if (entity == null) return;

            entity.Status = "OLD";
            database.Events.Update(entity);
            database.SaveChanges();
        }

        public static void UpdateEventStatusToOldWithTime(EventModel evnt, string sensorValue)
        {
            using TfortisdbContext database = new();
            var entity = database.Events.Where(t => t.Time == evnt.Time && t.DeviceName == evnt.DeviceName && t.SensorName == evnt.SensorName && t.Ip == evnt.Ip && t.SensorValueText == sensorValue && t.Status != "OLD").FirstOrDefault();
            entity.Status = "OLD";
            database.SaveChanges();
        }
        
        public static void SetOldAllEvent(EventModel evnt)
        {
            using TfortisdbContext database = new();
       
            var rowsToUpdate = database.Events.Where(t => t.DeviceName == evnt.DeviceName && t.Ip == evnt.Ip && t.Status != "OLD");

            foreach(var e in rowsToUpdate)
            {
                e.Status = "OLD";
                database.Events.Update(e);
            }

            database.SaveChanges();
        }
        
        public static void SetOldAllEvent(MonitoringDevice device)
        {
            using TfortisdbContext database = new();
            var eventsToUpdate = database.Events.Where(t => t.DeviceName == device.Name && t.Ip == device.IpAddress && t.Status != "OLD").ToList();

            foreach(var evnt in eventsToUpdate)
            {
                evnt.Status = "OLD";
                database.Events.Update(evnt);
                database.SaveChanges();
            }
        }
                
        public static int SetOldAllEvent()
        {
            using TfortisdbContext database = new();
            int updatedRows = database.Database.ExecuteSqlRaw("UPDATE Events SET status = 'OLD'");
            return updatedRows;
        }
       
        #region Вложенные типы
        public class SensorForMonitoring
        {
            public string? DeviceName { get; set; }
            public string? DeviceIp { get; set; }
            public string? DeviceMac { get; set; }
            public string? DeviceLocation { get; set; }
            public string? DeviceDescription { get; set; }
            public string? Community { get; set; }
            public bool SendTheEmailIfAvailable { get; set; }
            public bool SendEmail { get; set; }
            public string? SensorName { get; set; }
            public string? SensorDescription { get; set; }
            public int Timeout { get; set; }
            public bool Invert { get; set; }
            public bool Enable { get; set; }
        }
        #endregion

        public static PhysicalAddress Parse(string data)
        {
            var bytes = new List<byte>();
            foreach (var b in data.Split(':'))
            {
                bytes.Add(byte.Parse(b, NumberStyles.HexNumber, CultureInfo.InvariantCulture));
            };
            return new PhysicalAddress(bytes.ToArray());
        }
    }
}
