package antCode;


/**
 * base class for an ant.
 * does everything related to network and parameter updating.
 * 
 * @author Felix Dietrich
 *
 */
public class Ameise extends GameObject {
	public String commands = "";

	public int AktuelleEnergie;
	public int AktuelleGeschwindigkeit;
	public boolean Angekommen;
	public int Angriff;
	public int AnzahlAmeisenDerSelbenKasteInSichtweite;
	public int AnzahlAmeisenDesTeamsInSichtweite;
	public int AnzahlAmeisenInSichtweite;
	public int AnzahlFremderAmeisenInSichtweite;
	public boolean IstMuede;
	public int Richtung;
	public int Sichtweite;
	public int WanzenInSichtweite;
	public int ZurueckgelegteStrecke;

	public int AktuelleLast;
	public Obst GetragenesObst;
	public int MaximaleLast;
	public int Reichweite;
	public String Volk;

	public int Ziel;

	public Ameise(int objNo) {
		super(objNo);
	}

	/**
	 * sets all ant parameters based on the given network string.
	 * calls the respective function that was called originally by the simulation and generates
	 * the commands the programmer of MeineAmeise defined.
	 * 
	 * @param nMessage
	 *            format: key:value,key:value,...
	 */
	public Ameise(String[] nMessage) {
		super(0);

		int objNo = 0;
		String objParam1 = "";
		String function = "";
		for (String keyvalue : nMessage) {
			String[] kvSplit = keyvalue.split(":");
			if(kvSplit.length == 0 || kvSplit[0].length() == 0)
				continue;
			MessageKey key = MessageKey.valueOf(kvSplit[0]);
			switch (key) {
			case gameObject:
				String[] objInfo = kvSplit[1].split("#");
				if(objInfo.length >= 1)
					objNo = Integer.parseInt(objInfo[0]);
				if(objInfo.length >= 2)
					objParam1 = objInfo[1];
				if(objInfo.length >= 3) {
				}
				break;
			case function:
				function = kvSplit[1];
				break;
			case AktuelleEnergie:
				this.AktuelleEnergie = Integer.parseInt(kvSplit[1]);
				break;
			case AktuelleGeschwindigkeit:
				this.AktuelleGeschwindigkeit = Integer.parseInt(kvSplit[1]);
				break;
			case AktuelleLast:
				this.AktuelleLast = Integer.parseInt(kvSplit[1]);
				break;
			case Angekommen:
				this.Angekommen = Boolean.parseBoolean(kvSplit[1]);
				break;
			case Angriff:
				this.Angriff = Integer.parseInt(kvSplit[1]);
				break;
			case AnzahlAmeisenDerSelbenKasteInSichtweite:
				this.AnzahlAmeisenDerSelbenKasteInSichtweite = Integer
						.parseInt(kvSplit[1]);
				break;
			case AnzahlAmeisenDesTeamsInSichtweite:
				this.AnzahlAmeisenDesTeamsInSichtweite = Integer
						.parseInt(kvSplit[1]);
				break;
			case AnzahlAmeisenInSichtweite:
				this.AnzahlAmeisenInSichtweite = Integer.parseInt(kvSplit[1]);
				break;
			case AnzahlFremderAmeisenInSichtweite:
				this.AnzahlFremderAmeisenInSichtweite = Integer
						.parseInt(kvSplit[1]);
				break;
			case IstMuede:
				this.IstMuede = Boolean.parseBoolean(kvSplit[1]);
				break;
			case Reichweite:
				this.Reichweite = Integer.parseInt(kvSplit[1]);
				break;
			case Richtung:
				this.Richtung = Integer.parseInt(kvSplit[1]);
				break;
			case Sichtweite:
				this.Sichtweite = Integer.parseInt(kvSplit[1]);
				break;
			case WanzenInSichtweite:
				this.WanzenInSichtweite = Integer.parseInt(kvSplit[1]);
				break;
			case ZurueckgelegteStrecke:
				this.ZurueckgelegteStrecke = Integer.parseInt(kvSplit[1]);
				break;
			case Ziel:
				this.Ziel = Integer.parseInt(kvSplit[1]);
				break;
			}
		}
		// apply the function after clearing all commands
		clearCommands();
		if (function.length() > 0) {
			FunctionKey fkey = FunctionKey.valueOf(function);
			switch (fkey) {
			case Wartet:
				this.Wartet();
				break;
			case WirdMuede:
				this.WirdMuede();
				break;
			case SiehtZucker:
				this.Sieht(new Zucker(objNo, Integer.parseInt(objParam1)));
				break;
			case SiehtObst:
				this.Sieht(new Obst(objNo, Integer.parseInt(objParam1)));
				break;
			case ZielErreichtZucker:
				this.ZielErreicht(new Zucker(objNo, Integer.parseInt(objParam1)));
				break;
			case ZielErreichtObst:
				this.ZielErreicht(new Obst(objNo, Integer.parseInt(objParam1)));
				break;
			case RiechtFreund:
				this.RiechtFreund(new Markierung(objNo, Integer.parseInt(objParam1)));
				break;
			case SiehtFreund:
				this.SiehtFreund(new Ameise(objNo));
				break;
			case SiehtVerbuendeten:
				this.SiehtVerbuendeten(new Ameise(objNo));
				break;
			case SiehtFeindWanze:
				this.SiehtFeind(new Wanze(objNo, Integer.parseInt(objParam1)));
				break;
			case SiehtFeindAmeise:
				this.SiehtFeind(new Ameise(objNo));
				break;
			case WirdAngegriffenWanze:
				this.WirdAngegriffen(new Wanze(objNo, Integer.parseInt(objParam1)));
				break;
			case IstGestorben:
				this.IstGestorben();
				break;
			case BestimmeKaste:
				this.ReturnKaste();
				break;
			}
		}
	}

	// //////////////////////////////////
	// // network commands, piling up and sent after one standard ant function returns. they are executed in the simulation.

	public String getCommands() {
		return commands;
	}

	private void addCommand(String c) {
		this.commands += c + ",";
	}

	private void clearCommands() {
		this.commands = "";
	}

	// ///////////////////////////////////
	// / this functions can be changed by the "user" of this class, i.e. the programmer of MeineAmeise.


	//#region Kaste

	/// <summary>
	/// Bestimmt die Kaste einer neuen Ameise.
	/// </summary>
	/// <param name="anzahl">Die Anzahl der von jeder Kaste bereits vorhandenen
	/// Ameisen.</param>
	/// <returns>Der Name der Kaste der Ameise.</returns>
	private void ReturnKaste()
    {
		addCommand("ReturnKaste " + BestimmeKaste());
	}
	
	public String BestimmeKaste()
    {
		return "Standard";
	}

	// / <summary>
	// / Wird wiederholt aufgerufen, wenn der die Ameise nicht weiss wo sie
	// / hingehen soll.
	// / </summary>
	public void Wartet() {
	}

	// / <summary>
	// / Wird einmal aufgerufen, wenn die Ameise ein Drittel ihrer maximalen
	// / Reichweite ueberschritten hat.
	// / </summary>
	public void WirdMuede() {
	}

	// #region Nahrung

	// / <summary>
	// / Wird wiederholt aufgerufen, wenn die Ameise mindestens einen
	// / Zuckerhaufen sieht.
	// / </summary>
	// / <param name="zucker">Der nächstgelegene Zuckerhaufen.</param>
	public void Sieht(Zucker zucker) {
	}

	// / <summary>
	// / Wird wiederholt aufgerufen, wenn die Ameise mindstens ein
	// / Obststueck sieht.
	// / </summary>
	// / <param name="obst">Das nächstgelegene Obststueck.</param>
	public void Sieht(Obst obst) {
	}

	// / <summary>
	// / Wird einmal aufgerufen, wenn di e Ameise einen Zuckerhaufen als Ziel
	// / hat und bei diesem ankommt.
	// / </summary>
	// / <param name="zucker">Der Zuckerhaufen.</param>
	public void ZielErreicht(Zucker zucker) {
	}

	// / <summary>
	// / Wird einmal aufgerufen, wenn die Ameise ein Obststueck als Ziel hat und
	// / bei diesem ankommt.
	// / </summary>
	// / <param name="obst">Das Obstueck.</param>
	public void ZielErreicht(Obst obst) {
	}

	// #region Kommunikation

	// / <summary>
	// / Wird einmal aufgerufen, wenn die Ameise eine Markierung des selben
	// / Volkes riecht. Einmal gerochene Markierungen werden nicht erneut
	// / gerochen.
	// / </summary>
	// / <param name="markierung">Die nächste neue Markierung.</param>
	public void RiechtFreund(Markierung markierung) {
	}

	// / <summary>
	// / Wird wiederholt aufgerufen, wenn die Ameise mindstens eine Ameise des
	// / selben Volkes sieht.
	// / </summary>
	// / <param name="ameise">Die nächstgelegene befreundete Ameise.</param>
	public void SiehtFreund(Ameise ameise) {
	}

	// / <summary>
	// / Wird aufgerufen, wenn die Ameise eine befreundete Ameise eines anderen
	// Teams trifft.
	// / </summary>
	// / <param name="ameise"></param>
	public void SiehtVerbuendeten(Ameise ameise) {
	}

	// #region Kampf

	// / <summary>
	// / Wird wiederholt aufgerufen, wenn die Ameise mindestens eine Wanze
	// / sieht.
	// / </summary>
	// / <param name="wanze">Die nächstgelegene Wanze.</param>
	public void SiehtFeind(Wanze wanze) {
	}

	// / <summary>
	// / Wird wiederholt aufgerufen, wenn die Ameise mindestens eine Ameise
	// eines
	// / anderen Volkes sieht.
	// / </summary>
	// / <param name="ameise">Die nächstgelegen feindliche Ameise.</param>
	public void SiehtFeind(Ameise ameise) {
	}

	// / <summary>
	// / Wird wiederholt aufgerufen, wenn die Ameise von einer Wanze angegriffen
	// / wird.
	// / </summary>
	// / <param name="wanze">Die angreifende Wanze.</param>
	public void WirdAngegriffen(Wanze wanze) {
	}

	// / <summary>
	// / Wird wiederholt aufgerufen in der die Ameise von einer Ameise eines
	// / anderen Volkes Ameise angegriffen wird.
	// / </summary>
	// / <param name="ameise">Die angreifende feindliche Ameise.</param>
	public void WirdAngegriffen(Ameise ameise) {
	}

	// #region Sonstiges

	// / <summary>
	// / Wird einmal aufgerufen, wenn die Ameise gestorben ist.
	// / </summary>
	// / <param name="todesart">Die Todesart der Ameise</param>
	public void IstGestorben() {
	}

	// /////////////////////////////////
	// functions that can be called from the functions above but should not be modifed.

	public void BleibStehen() {
		addCommand("BleibStehen");
	}

	public void DreheInRichtung(int richtung) {
		addCommand("DreheInRichtung " + richtung);
	}

	public void DreheUm() {
		addCommand("DreheUm");
	}

	public void DreheUmWinkel(int winkel) {
		addCommand("DreheUmWinkel " + winkel);
	}

	public void DreheZuZiel(GameObject ziel) {
		addCommand("DreheZuZiel " + ziel.id);
	}

	public void GeheGeradeaus() {
		addCommand("GeheGeradeaus");
	}

	public void GeheWegVon(GameObject ziel) {
		addCommand("GeheWegVon " + ziel.id);
	}

	public void GeheZuZiel(GameObject ziel) {
		addCommand("GeheZuZiel " + ziel.id);
	}

	public void GeheZuBau() {
		addCommand("GeheZuBau");
	}

	public void GreifeAn(GameObject ziel) {
		addCommand("GreifeAn " + ziel.id);
	}

	public void LasseNahrungFallen() {
		addCommand("LasseNahrungFallen");
	}

	public void Nimm(Zucker zucker) {
		addCommand("Nimm " + zucker.id);
	}

	public void Nimm(Obst obst) {
		addCommand("Nimm " + obst.id);
	}

	public void SprueheMarkierung(int information) {
		addCommand("SprueheMarkierung " + information);
	}

	public void SprueheMarkierung(int information, int groesse) {
		addCommand("SprueheMarkierung " + information + " " + groesse);
	}
}
