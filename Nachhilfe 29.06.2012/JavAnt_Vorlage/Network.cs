using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace Useful
{
    public class Network
    {
        public static string receive(TcpClient client)
        {
            var stream = client.GetStream();
            byte[] buffer = new byte[4];
            var readBytes = stream.Read(buffer, 0, 4);
            if (readBytes == 0)
            {
                throw new IOException("connection to server lost");
            }
            else
            {
                // cruel but I dont have the time to check how to do this with the API
                int msgSize = buffer[0] * 256*256*256 + buffer[1] * 256*256 + buffer[2] * 256 + buffer[3];
                buffer = new byte[msgSize];
                readBytes = stream.Read(buffer, 0, msgSize);
                if (readBytes == 0)
                    throw new IOException("connection to server lost");
                else
                {
                    var msg = System.Text.Encoding.ASCII.GetString(buffer);
                    return msg;
                }
            }
        }

        public static void send(TcpClient client, string msg)
        {
            //printfn "sent %s" msg
            var stream = client.GetStream();
            var msgBuffer = Encoding.ASCII.GetBytes(msg);
            var buffer = BitConverter.GetBytes(msgBuffer.Length);
            stream.Write(buffer, 0, 4);
            stream.Write(msgBuffer, 0, msgBuffer.Length);
            stream.Flush();
        }

        public static string state2string(AntMe.Spieler.MeineAmeise ant)
        {
            // generate list of attributes, 
            // join attributes into json like object
            string attributes = "";
            try
            {
                attributes += String.Format("{0}:{1},", "AktuelleEnergie", ant.AktuelleEnergie.ToString());
                attributes += String.Format("{0}:{1},", "AktuelleGeschwindigkeit", ant.AktuelleGeschwindigkeit.ToString());
                attributes += String.Format("{0}:{1},", "AktuelleLast", ant.AktuelleLast.ToString());
                attributes += String.Format("{0}:{1},", "Angekommen", ant.Angekommen.ToString());
                attributes += String.Format("{0}:{1},", "Angriff", ant.Angriff.ToString());
                attributes += String.Format("{0}:{1},", "AnzahlAmeisenDerSelbenKasteInSichtweite", ant.AnzahlAmeisenDerSelbenKasteInSichtweite.ToString());
                attributes += String.Format("{0}:{1},", "AnzahlAmeisenDesTeamsInSichtweite", ant.AnzahlAmeisenDesTeamsInSichtweite.ToString());
                attributes += String.Format("{0}:{1},", "AnzahlAmeisenInSichtweite", ant.AnzahlAmeisenInSichtweite.ToString());
                attributes += String.Format("{0}:{1},", "AnzahlFremderAmeisenInSichtweite", ant.AnzahlFremderAmeisenInSichtweite.ToString());
                attributes += String.Format("{0}:{1},", "IstMuede", ant.IstMüde.ToString());
                attributes += String.Format("{0}:{1},", "Reichweite", ant.Reichweite.ToString());
                attributes += String.Format("{0}:{1},", "Richtung", ant.Richtung.ToString());
                attributes += String.Format("{0}:{1},", "Sichtweite", ant.Sichtweite.ToString());
                attributes += String.Format("{0}:{1},", "WanzenInSichtweite", ant.WanzenInSichtweite.ToString());
                attributes += String.Format("{0}:{1},", "ZurueckgelegteStrecke", ant.ZurückgelegteStrecke.ToString());

                if (ant.Ziel != null) attributes += String.Format("{0}:{1},", "Ziel", AntMe.Spieler.MeineAmeise.addTarget(ant.Ziel));
                else attributes += String.Format("{0}:{1},", "Ziel", 0);
                if (ant.GetragenesObst != null) attributes += String.Format("{0}:{1},", "GetragenesObst", ant.GetragenesObst.ToString());
                if (ant.Kaste != null) attributes += String.Format("{0}:{1},", "Kaste", ant.Kaste.ToString());
            }
            catch (NullReferenceException)
            {
                return String.Format("{0}:{1},", "AktuelleEnergie", ant.AktuelleEnergie.ToString());
            }
            return attributes;
        }

        internal static string state2string(string function, AntMe.Spieler.MeineAmeise meineAmeise)
        {
            return String.Format("{0},function:{1}", state2string(meineAmeise), function);
        }

        /// <summary>
        /// converts game data into a string that can be sent over the network.
        /// format: key:value, key:value, ...
        /// </summary>
        /// <param name="function">ant function to call on the server</param>
        /// <param name="objNo">[optional] object no of object in parameter</param>
        /// <param name="objParam1">[optional] parameters of the object</param>
        /// <param name="objParam2">[optional] parameters of the object</param>
        /// <param name="meineAmeise">the current ant where most info is extracted</param>
        /// <returns></returns>
        internal static string state2string(string function, int objNo, string objParam1, string objParam2, AntMe.Spieler.MeineAmeise meineAmeise)
        {
            if (objParam1 != null && objParam2 == null)
                return String.Format("gameObject:{0}#{1},{2},function:{3}", objNo, objParam1, state2string(meineAmeise), function);
            if (objParam1 != null && objParam2 != null)
                return String.Format("gameObject:{0}#{1}#{2},{3},function:{4}", objNo, objParam1, objParam2, state2string(meineAmeise), function);
            return String.Format("gameObject:{0},{1},function:{2}", objNo, state2string(meineAmeise), function);
        }
    }
}
