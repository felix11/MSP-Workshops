
trait Ordered[A]
{
  def compare(that: A): Int
  def <(that: A): Boolean = (this compare that) < 0
  def >(that: A): Boolean = (this compare that) > 0
  def <=(that: A): Boolean = (this compare that) <= 0
  def >=(that: A): Boolean = (this compare that) >= 0
  def ===(that: A): Boolean = (this compare that) == 0
}

trait PrettyPrint
{
	def toPrettyString : String = "---\n" + this.toString + "\n---"
}

class Rational(n: Int, d: Int) extends Object with Ordered[Rational] with PrettyPrint {
  override def compare(r: Rational): Int =
    {
      val n1 = this.numer * r.denom
      val n2 = r.numer * this.denom
      (n1 - n2) match
        {
          case x if x > 0 => 1
          case x if x < 0 => -1
          case _ => 0
        }
    }

  private def gcd(x: Int, y: Int): Int =
    {
      if (x == 0) y
      else if (x < 0) gcd(-x, y)
      else if (y < 0) -gcd(x, -y)
      else gcd(y % x, x)
    }

  private val g = gcd(n, d)
  val numer: Int = n / g
  val denom: Int = d / g

  def +(that: Rational) =
    new Rational(numer * that.denom + that.numer * denom,
      denom * that.denom)

  def -(that: Rational) =
    new Rational(numer * that.denom - that.numer * denom,
      denom * that.denom)

  def *(that: Rational) =
    new Rational(numer * that.numer, denom * that.denom)

  def /(that: Rational) =
    new Rational(numer * that.denom, denom * that.numer)

  override def toString(): String =
    this.numer + "/" + this.denom
}

