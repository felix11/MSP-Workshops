using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace Useful
{
    public class Network
    {
        public string receive(TcpClient client)
        {
            try
            {
                var stream = client.GetStream();
                byte[] buffer = new byte[4];
                var readBytes = stream.Read(buffer, 0, 4);
                if (readBytes == 0)
                {
                    throw new Exception("connection to server lost");
                }
                else
                {
                    int msgSize = BitConverter.ToInt32(buffer, 0);
                    buffer = new byte[msgSize];
                    readBytes = stream.Read(buffer, 0, msgSize);
                    if (readBytes == 0)
                        throw new Exception("connection to server lost");
                    else
                    {
                        var msg = System.Text.Encoding.Unicode.GetString(buffer);
                        return msg;
                    }
                }
            }
            catch (Exception e)
            {

            }
            return "";
        }

        public void send(TcpClient client, string msg)
        {
            //printfn "sent %s" msg
            var stream = client.GetStream();
            var msgBuffer = Encoding.Unicode.GetBytes(msg);
            var buffer = BitConverter.GetBytes(msgBuffer.Length);
            stream.Write(buffer, 0, 4);
            stream.Flush();
            stream.Write(msgBuffer, 0, msgBuffer.Length);
            stream.Flush();
        }
    }
}
