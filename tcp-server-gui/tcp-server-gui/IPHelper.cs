using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace tcp_server_gui
{
    class IPHelper
    { 
        public static byte[] MsgToByte(string msg)
        {
            return Encoding.ASCII.GetBytes(msg);
        }
        public static List<IPAddressDetail> GetInterfaceIPAddress(bool debug = false)
        {
            var nics = NetworkInterface.GetAllNetworkInterfaces();
            // Using struct for making data 
            var IPAddresses = new List<IPAddressDetail>();

            foreach (var nic in nics)
            {
                var ipProps = nic.GetIPProperties();

                // We're only interested in IPv4 addresses for this example.
                var ipv4Addrs = ipProps.UnicastAddresses
                    .Where(addr => addr.Address.AddressFamily == AddressFamily.InterNetwork)
                    .Where(addr => addr.Address.ToString() != "127.0.0.1") // Exclude localhost
                    .Where(addr => addr.Address.ToString().Substring(0, 7) != "169.254"); // Exclude DHCP IP, Windows based

                foreach (var addr in ipv4Addrs)
                {
                    var network = CalculateNetwork(addr);
                    var broadcast = GetBroadcastAddress(addr);

                    if (network != null)
                    {
                        if (debug)
                            Console.WriteLine("Addr: {0}   Mask: {1}  Network: {2} Broadcast : {3}", addr.Address, addr.IPv4Mask, network, broadcast);
                        IPAddresses.Add(new IPAddressDetail(addr: addr.Address.ToString(), broadcast: broadcast.ToString(), mask: addr.IPv4Mask.ToString(), unicast: addr));
                    }
                }
            }

            return IPAddresses;
        }

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
    public struct IPAddressDetail
    {
        public IPAddressDetail(string addr, string broadcast, string mask, UnicastIPAddressInformation unicast)
        {
            address = addr;
            this.broadcast = broadcast;
            this.mask = mask;
            this.unicast = unicast;
        }

        public string address;
        public string broadcast;
        public string mask;
        public UnicastIPAddressInformation unicast;

    }
}
