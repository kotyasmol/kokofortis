using Microsoft.EntityFrameworkCore;

namespace TFortisDeviceManagerSetup;

public partial class TfortisdbContext : DbContext
{
    public TfortisdbContext()
    {

    }
    public TfortisdbContext(DbContextOptions<TfortisdbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<DeviceAndOid> DeviceAndOids { get; set; }

    public virtual DbSet<DeviceForMonitoring> DeviceForMonitoring { get; set; }

    public virtual DbSet<DeviceType> DeviceTypes { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<OidsForDevice> OidsForDevices { get; set; }


    public virtual DbSet<TrapOid> TrapOids { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=tfortis;Username=postgres;Password=tfortis");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DeviceAndOid>(entity =>
        {
            entity.HasKey(e => e.Key).HasName("DeviceAndOids_pkey");

            entity.Property(e => e.Key).HasColumnName("key");
            entity.Property(e => e.DeviceForMonitoringKey).HasColumnName("deviceForMonitoringKey");
            entity.Property(e => e.Enable).HasColumnName("enable");
            entity.Property(e => e.Invert)
                .HasDefaultValueSql("false")
                .HasColumnName("invert");
            entity.Property(e => e.Invertible)
                .HasDefaultValueSql("false")
                .HasColumnName("invertible");
            entity.Property(e => e.OidForDeviceKey).HasColumnName("oidForDeviceKey");
            entity.Property(e => e.SendEmail).HasColumnName("sendEmail");
            entity.Property(e => e.Timeout).HasColumnName("timeout");
        });

        modelBuilder.Entity<DeviceForMonitoring>(entity =>
        {
            entity.HasKey(e => e.Key).HasName("DeviceForMonitoring_pkey");

            entity.ToTable("DeviceForMonitoring");

            entity.Property(e => e.Key)
                .HasColumnName("key");
            entity.Property(e => e.Community).HasColumnName("community");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DeviceTypeId).HasColumnName("deviceTypeId");
            entity.Property(e => e.Ip).HasColumnName("ip");
            entity.Property(e => e.Location).HasColumnName("location");
            entity.Property(e => e.Mac).HasColumnName("mac");
            entity.Property(e => e.SendEmail).HasColumnName("sendEmail");
            entity.Property(e => e.Uptime).HasColumnName("uptime");
        });

        modelBuilder.Entity<DeviceType>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Inputs).HasColumnName("inputs");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Outputs).HasColumnName("outputs");
            entity.Property(e => e.PortsPoe).HasColumnName("portsPoe");
            entity.Property(e => e.PortsSfp).HasColumnName("portsSfp");
            entity.Property(e => e.PortsUplink).HasColumnName("portsUplink");
            entity.Property(e => e.PortsWithoutPoe).HasColumnName("portsWithoutPoe");
            entity.Property(e => e.Ups).HasColumnName("ups");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Events_pkey");

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DeviceDescription).HasColumnName("deviceDescription");
            entity.Property(e => e.DeviceLocation).HasColumnName("deviceLocation");
            entity.Property(e => e.DeviceName).HasColumnName("deviceName");
            entity.Property(e => e.Ip).HasColumnName("ip");
            entity.Property(e => e.SensorName).HasColumnName("sensorName");
            entity.Property(e => e.SensorValueText).HasColumnName("sensorValueText");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Time).HasColumnName("time");
        });

        modelBuilder.Entity<OidsForDevice>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("OidsForDevice");

            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.BadValue).HasColumnName("badValue");
            entity.Property(e => e.BadValueText).HasColumnName("badValueText");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DeviceTypeId).HasColumnName("deviceTypeId");
            entity.Property(e => e.Enable).HasColumnName("enable");
            entity.Property(e => e.Invertible).HasColumnName("invertible");
            entity.Property(e => e.Key).HasColumnName("key");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.OkValue).HasColumnName("okValue");
            entity.Property(e => e.OkValueText).HasColumnName("okValueText");
            entity.Property(e => e.Timeout).HasColumnName("timeout");
        });

        modelBuilder.Entity<TrapOid>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.BadValue).HasColumnName("badValue");
            entity.Property(e => e.BadValueText).HasColumnName("badValueText");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Key).HasColumnName("key");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.OkValue).HasColumnName("okValue");
            entity.Property(e => e.OkValueText).HasColumnName("okValueText");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
