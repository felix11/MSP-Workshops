
import scala.actors.Actor
import scala.actors.Actor._

case class Ping(receiver:Actor)
case class Pong

class Pinger(name:String) extends Actor
{
	def act()
	{
		loop
		{
			react
			{
				case Ping(a) =>
				{
					println(name + ": ping")
					Thread.sleep(500)
					a ! Pong
				}
				case Pong =>
				{
					println(name + ": pong")
					exit()
				}
			}
		}
	}
}

object ActorTest
{
	def start()
	{
		val a1 = new Pinger("a1")
		val a2 = new Pinger("a2")
		a1.start
		a2.start
		
		a1 ! Ping(a2)
		a2 ! Ping(a1)
	}
}