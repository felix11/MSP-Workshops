package antCode;

public class MeineAmeise extends Ameise {
	// #region Fortbewegung

	public MeineAmeise(String[] nMessage) {
		super(nMessage);
	}
	
	//#region Kaste

	/// <summary>
	/// Bestimmt die Kaste einer neuen Ameise.
	/// </summary>
	/// <param name="anzahl">Die Anzahl der von jeder Kaste bereits vorhandenen
	/// Ameisen.</param>
	/// <returns>Der Name der Kaste der Ameise.</returns>
	public String BestimmeKaste()
    {
		return "Standard";
	}

	// / <summary>
	// / Wird wiederholt aufgerufen, wenn der die Ameise nicht weiss wo sie
	// / hingehen soll.
	// / </summary>
	public void Wartet() {
		this.GeheGeradeaus();
	}

	// / <summary>
	// / Wird einmal aufgerufen, wenn die Ameise ein Drittel ihrer maximalen
	// / Reichweite ueberschritten hat.
	// / </summary>
	public void WirdMuede() {
		if(this.Ziel == 0)
		{
			this.GeheZuBau();
		}
	}

	// #region Nahrung

	// / <summary>
	// / Wird wiederholt aufgerufen, wenn die Ameise mindestens einen
	// / Zuckerhaufen sieht.
	// / </summary>
	// / <param name="zucker">Der nächstgelegene Zuckerhaufen.</param>
	public void Sieht(Zucker zucker) {
		if(this.AktuelleLast == 0 && this.Ziel == 0)
		{
			this.GeheZuZiel(zucker);
		}
	}

	// / <summary>
	// / Wird wiederholt aufgerufen, wenn die Ameise mindstens ein
	// / Obststueck sieht.
	// / </summary>
	// / <param name="obst">Das nächstgelegene Obststueck.</param>
	public void Sieht(Obst obst) {
		if(this.AktuelleLast == 0 && this.Ziel == 0)
		{
			this.GeheZuZiel(obst);
		}
	}

	// / <summary>
	// / Wird einmal aufgerufen, wenn di e Ameise einen Zuckerhaufen als Ziel
	// / hat und bei diesem ankommt.
	// / </summary>
	// / <param name="zucker">Der Zuckerhaufen.</param>
	public void ZielErreicht(Zucker zucker) {
		this.Nimm(zucker);
		this.GeheZuBau();
		this.SprueheMarkierung(10, 300);
	}

	// / <summary>
	// / Wird einmal aufgerufen, wenn die Ameise ein Obststueck als Ziel hat und
	// / bei diesem ankommt.
	// / </summary>
	// / <param name="obst">Das Obstueck.</param>
	public void ZielErreicht(Obst obst) {
		this.Nimm(obst);
		this.GeheZuBau();
	}

	// #region Kommunikation

	// / <summary>
	// / Wird einmal aufgerufen, wenn die Ameise eine Markierung des selben
	// / Volkes riecht. Einmal gerochene Markierungen werden nicht erneut
	// / gerochen.
	// / </summary>
	// / <param name="markierung">Die nächste neue Markierung.</param>
	public void RiechtFreund(Markierung markierung) {
		switch(markierung.Information)
		{
		case 10:
			if(this.AktuelleLast == 0 && this.Ziel == 0)
				this.GeheZuZiel(markierung);
			break;
		case 11:
			if(this.AktuelleLast == 0 && this.Ziel == 0)
				this.GeheZuZiel(markierung);
			break;
		}
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
		if(this.AnzahlAmeisenDesTeamsInSichtweite > 0 && this.AktuelleLast == 0)
		{
			this.SprueheMarkierung(11, 100);
			this.GreifeAn(wanze);
		}
			
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
		if( this.AktuelleLast == 0)
		{
			this.GreifeAn(wanze);
		}
		this.SprueheMarkierung(11, 150);
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

}
