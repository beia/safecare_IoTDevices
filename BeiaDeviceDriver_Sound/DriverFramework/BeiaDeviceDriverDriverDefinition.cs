using System;
using System.Collections.Generic;
using System.Security;
using VideoOS.Platform.DriverFramework;
using VideoOS.Platform.DriverFramework.Data.Settings;
using VideoOS.Platform.DriverFramework.Definitions;

namespace Safecare.BeiaDeviceDriver_Sound
{
    /// <summary>
    /// The main entry point for the device driver.
    /// </summary>
    public class BeiaDeviceDriver_SoundDriverDefinition : DriverDefinition
    {
        /// <summary>
        /// Create session to device, or throw exceptions if not successful
        /// </summary>
        /// <param name="uri">The URI specified by the operator when adding the device</param>
        /// <param name="userName">The user name specified</param>
        /// <param name="password">The password specified</param>
        /// <returns>Container representing a device</returns>
        protected override Container CreateContainer(Uri uri, string userName, SecureString password, ICollection<HardwareSetting> hardwareSettings)
        {
            return new BeiaDeviceDriver_SoundContainer(this);
        }

        protected override DriverInfo CreateDriverInfo()
        {
            // TODO: Replace values below with values describing your driver.
            return new DriverInfo(Constants.DriverId, "BeiaDeviceDriver_Sound", "BeiaDeviceDriver_Sound group", "1.0", new[] { Constants.Product1 });
        }
    }
}
