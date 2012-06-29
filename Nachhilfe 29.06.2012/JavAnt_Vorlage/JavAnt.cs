using System;
using System.Collections.Generic;

using AntMe.Deutsch;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Useful;

// Fuege hier hinter AntMe.Spieler einen Punkt und deinen Namen ohne Leerzeichen
// ein! Zum Beispiel "AntMe.Spieler.WolfgangGallo".
namespace AntMe.Spieler
{

	// Das Spieler-Attribut erlaubt das Festlegen des Volk-Names und von Vor-
	// und Nachname des Spielers. Der Volk-Name muß zugewiesen werden, sonst wird
	// das Volk nicht gefunden.
	[Spieler(
		Volkname = "Java Ants!",
		Vorname = "",
		Nachname = ""
	)]

	// Das Typ-Attribut erlaubt das Ändern der Ameisen-Eigenschaften. Um den Typ
	// zu aktivieren muß ein Name zugewiesen und dieser Name in der Methode 
	// BestimmeTyp zurueckgegeben werden. Das Attribut kann kopiert und mit
	// verschiedenen Namen mehrfach verwendet werden.
	// Eine genauere Beschreibung gibts in Lektion 6 des Ameisen-Tutorials.
	[Kaste(
		Name = "Standard",
		GeschwindigkeitModifikator = 0,
		DrehgeschwindigkeitModifikator = 0,
		LastModifikator = 0,
		ReichweiteModifikator = 0,
		SichtweiteModifikator = 0,
		EnergieModifikator = 0,
		AngriffModifikator = 0
	)]

	public class MeineAmeise : Basisameise
	{
        private TcpClient client;
        private static Dictionary<Spielobjekt, int> targets = new Dictionary<Spielobjekt, int>();
        private static int nextObject = 0;
        private string returnKaste;

        private static int incTargets()
        {
            return nextObject++;
        }

        public MeineAmeise()
        {
            client = new TcpClient("localhost", 8888);
        }

        /// <summary>
        /// executes network commands on this ant.
        /// </summary>
        /// <param name="cmds"></param>
        private void execCommands(string cmds)
        {
            string[] cmdsSplit = cmds.Split(',');
            foreach (string cmd in cmdsSplit)
            {
                string[] cmdAndParam = cmd.Split(' ');
                int objNo;
                switch (cmdAndParam[0])
                {
                    case "BleibStehen":
                            this.BleibStehen();
                        break;
                    case "DreheInRichtung":
                        this.DreheInRichtung(int.Parse(cmdAndParam[1]));
                        break;
                    case "DreheUm":
                        this.DreheUm();
                        break;
                    case "DreheUmWinkel":
                        this.DreheUmWinkel(int.Parse(cmdAndParam[1]));
                        break;
                    case "DreheZuZiel":
                        objNo = int.Parse(cmdAndParam[1]);
                        foreach(Spielobjekt s in targets.Keys)
                            if(objNo == targets[s])
                                this.DreheZuZiel(s);
                        break;
                    case "GeheGeradeaus":
                        if (cmdAndParam.Length == 1)
                            this.GeheGeradeaus();
                        else
                            this.GeheGeradeaus(int.Parse(cmdAndParam[1]));
                        break;
                    case "GeheWegVon":
                        objNo = int.Parse(cmdAndParam[1]);
                        foreach (Spielobjekt s in targets.Keys)
                            if (objNo == targets[s])
                                if(cmdAndParam.Length == 2)
                                    this.GeheWegVon(s);
                                else
                                    this.GeheWegVon(s,int.Parse(cmdAndParam[2]));
                        break;
                    case "GeheZuBau":
                        this.GeheZuBau();
                        break;
                    case "GeheZuZiel":
                        objNo = int.Parse(cmdAndParam[1]);
                        foreach (Spielobjekt s in targets.Keys)
                            if (objNo == targets[s])
                                this.GeheZuZiel(s);
                        break;
                    case "GreifeAn":
                        objNo = int.Parse(cmdAndParam[1]);
                        foreach (Spielobjekt s in targets.Keys)
                            if (objNo == targets[s])
                                this.GreifeAn(s as Insekt);
                        break;
                    case "LasseNahrungFallen":
                        this.LasseNahrungFallen();
                        break;
                    case "Nimm":
                        objNo = int.Parse(cmdAndParam[1]);
                        foreach (Spielobjekt s in targets.Keys)
                            if (objNo == targets[s])
                                this.Nimm(s as Nahrung);
                        break;
                    case "SprueheMarkierung":
                        if(cmdAndParam.Length == 2)
                            this.SprüheMarkierung(int.Parse(cmdAndParam[1]));
                        if(cmdAndParam.Length > 2)
                            this.SprüheMarkierung(int.Parse(cmdAndParam[1]), int.Parse(cmdAndParam[2]));
                        break;
                    case "ReturnKaste":
                        this.returnKaste = cmdAndParam[1];
                        break;
                }
            }
        }

        /// <summary>
        /// sends the current game state to the server and retrieves the command list for this ant.
        /// </summary>
        /// <param name="func">function that should be called remotely</param>
        /// <param name="obj">optional game object, e.g. sugar</param>
        private void networkCommands(string func, Spielobjekt obj = null, string objParam1 = null, string objParam2 = null)
        {
            if(obj != null)
                Network.send(client, Network.state2string(func, addTarget(obj), objParam1, objParam2, this));
            else
                Network.send(client, Network.state2string(func, this));
            string cmds = Network.receive(client);
            execCommands(cmds);
        }

        /// <summary>
        /// adds a game object to the game object list. used to handle remote references.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>an integer representing the object</returns>
        public static int addTarget(Spielobjekt obj)
        {
            if (targets.ContainsKey(obj))
                return targets[obj];
            else
            {
                int newTarget = incTargets();
                targets.Add(obj, newTarget);
                return newTarget;
            }
        }

		#region Kaste

		/// <summary>
		/// Bestimmt die Kaste einer neuen Ameise.
		/// </summary>
		/// <param name="anzahl">Die Anzahl der von jeder Kaste bereits vorhandenen
		/// Ameisen.</param>
		/// <returns>Der Name der Kaste der Ameise.</returns>
		public override string BestimmeKaste(Dictionary<string, int> anzahl)
        {
            networkCommands("BestimmeKaste");
            return this.returnKaste;
		}

		#endregion

		#region Fortbewegung

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn der die Ameise nicht weiss wo sie
		/// hingehen soll.
		/// </summary>
		public override void Wartet()
        {
            networkCommands("Wartet");
		}

		/// <summary>
		/// Wird einmal aufgerufen, wenn die Ameise ein Drittel ihrer maximalen
		/// Reichweite ueberschritten hat.
		/// </summary>
		public override void WirdMüde()
        {
            networkCommands("WirdMuede");
		}

		#endregion

		#region Nahrung

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise mindestens einen
		/// Zuckerhaufen sieht.
		/// </summary>
		/// <param name="zucker">Der nächstgelegene Zuckerhaufen.</param>
		public override void Sieht(Zucker zucker)
		{
            networkCommands("SiehtZucker", zucker, zucker.Menge.ToString());
		}

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise mindstens ein
		/// Obststueck sieht.
		/// </summary>
		/// <param name="obst">Das nächstgelegene Obststueck.</param>
		public override void Sieht(Obst obst)
        {
            networkCommands("SiehtObst", obst, obst.Menge.ToString());
		}

		/// <summary>
		/// Wird einmal aufgerufen, wenn di e Ameise einen Zuckerhaufen als Ziel
		/// hat und bei diesem ankommt.
		/// </summary>
		/// <param name="zucker">Der Zuckerhaufen.</param>
		public override void ZielErreicht(Zucker zucker)
        {
            networkCommands("ZielErreichtZucker", zucker, zucker.Menge.ToString());
		}

		/// <summary>
		/// Wird einmal aufgerufen, wenn die Ameise ein Obststueck als Ziel hat und
		/// bei diesem ankommt.
		/// </summary>
		/// <param name="obst">Das Obstueck.</param>
		public override void ZielErreicht(Obst obst)
        {
            networkCommands("ZielErreichtObst", obst, obst.Menge.ToString());
		}

		#endregion

		#region Kommunikation

		/// <summary>
		/// Wird einmal aufgerufen, wenn die Ameise eine Markierung des selben
		/// Volkes riecht. Einmal gerochene Markierungen werden nicht erneut
		/// gerochen.
		/// </summary>
		/// <param name="markierung">Die nächste neue Markierung.</param>
		public override void RiechtFreund(Markierung markierung)
        {
            networkCommands("RiechtFreund", markierung, markierung.Information.ToString());
		}

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise mindstens eine Ameise des
		/// selben Volkes sieht.
		/// </summary>
		/// <param name="ameise">Die nächstgelegene befreundete Ameise.</param>
		public override void SiehtFreund(Ameise ameise)
        {
            networkCommands("SiehtFreund", ameise);
		}

		/// <summary>
		/// Wird aufgerufen, wenn die Ameise eine befreundete Ameise eines anderen Teams trifft.
		/// </summary>
		/// <param name="ameise"></param>
		public override void SiehtVerbündeten(Ameise ameise)
        {
            networkCommands("SiehtVerbuendeten", ameise);
		}

		#endregion

		#region Kampf

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise mindestens eine Wanze
		/// sieht.
		/// </summary>
		/// <param name="wanze">Die nächstgelegene Wanze.</param>
		public override void SiehtFeind(Wanze wanze)
        {
            networkCommands("SiehtFeindWanze", wanze, wanze.AktuelleEnergie.ToString());
		}

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise mindestens eine Ameise eines
		/// anderen Volkes sieht.
		/// </summary>
		/// <param name="ameise">Die nächstgelegen feindliche Ameise.</param>
		public override void SiehtFeind(Ameise ameise)
        {
            networkCommands("SiehtFeindAmeise", ameise);
		}

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise von einer Wanze angegriffen
		/// wird.
		/// </summary>
		/// <param name="wanze">Die angreifende Wanze.</param>
		public override void WirdAngegriffen(Wanze wanze)
        {
            networkCommands("WirdAngegriffenWanze", wanze, wanze.AktuelleEnergie.ToString());
		}

		/// <summary>
		/// Wird wiederholt aufgerufen in der die Ameise von einer Ameise eines
		/// anderen Volkes Ameise angegriffen wird.
		/// </summary>
		/// <param name="ameise">Die angreifende feindliche Ameise.</param>
		public override void WirdAngegriffen(Ameise ameise)
        {
            networkCommands("WirdAngegriffenAmeise", ameise);
		}

		#endregion

		#region Sonstiges

		/// <summary>
		/// Wird einmal aufgerufen, wenn die Ameise gestorben ist.
		/// </summary>
		/// <param name="todesart">Die Todesart der Ameise</param>
		public override void IstGestorben(Todesart todesart)
        {
            networkCommands("IstGestorben");
		}

		/// <summary>
		/// Wird unabhängig von äußeren Umständen in jeder Runde aufgerufen.
		/// </summary>
		public override void Tick()
        {
            //networkCommands("Tick");
		}

		#endregion
		 
	}
}