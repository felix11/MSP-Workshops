package antCode;

public class Markierung extends GameObject {
	
	public Markierung(int objNo, int information) {
		super(objNo);
		
		this.Information = information;
	}
	
    public int Information;
}
