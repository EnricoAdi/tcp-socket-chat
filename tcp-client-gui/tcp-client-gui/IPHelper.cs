using System;
using System.Collections.Generic; 
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets; 
namespace tcp_client_gui
{
    class IPHelper
    {

        public static IPAddress GetBroadcastAddress(UnicastIPAddressInformation unicastAddress)
        {
            var address = unicastAddress.Address;
            var mask = unicastAddress.IPv4Mask;

            uint ipAddress = BitConverter.ToUInt32(address.GetAddressBytes(), 0);
            uint ipMaskV4 = BitConverter.ToUInt32(mask.GetAddressBytes(), 0);
            uint broadCastIpAddress = ipAddress | ~ipMaskV4;

            return new IPAddress(BitConverter.GetBytes(broadCastIpAddress));
        }

        public static IPAddress CalculateNetwork(UnicastIPAddressInformation addr)
        {
            // The mask will be null in some scenarios, like a dhcp address 169.254.x.x
            if (addr.IPv4Mask == null)
                return null;

            var ip = addr.Address.GetAddressBytes();
            var mask = addr.IPv4Mask.GetAddressBytes();
            var result = new Byte[4];
            for (int i = 0; i < 4; ++i)
            {
                result[i] = (Byte)(ip[i] & mask[i]);
            }

            return new IPAddress(result);
        }
    }

}
