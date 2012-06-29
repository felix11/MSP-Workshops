package server;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.SocketException;
import java.nio.ByteBuffer;

import antCode.MeineAmeise;

/**
 * server for communication between simulation and Java code.
 * 
 * @author Felix Dietrich
 *
 */
public class Server
{
	// entry point. starts the server
	public static void main(String[] args) {
        Server server = new Server();
        try {
            server.loop();
        } catch (IOException e) {
            e.printStackTrace();
        } 
    }
	
	/**
	 * main loop of the server.
	 * accepts clients and starts a thread for each one, handling the communication asynchronously.
	 * 
	 * @throws IOException
	 */
    void loop() throws IOException {
    	
    	// init server
        int port = 8888;
        java.net.ServerSocket serverSocket = new java.net.ServerSocket(port);
        
        // run indefinitely
        while(true)
        {
        	try
        	{
        		// accept clients
		        final java.net.Socket client = acceptNewClient(serverSocket);
		        
		        // handle communication asynchronously
		        Thread t = new Thread(new Runnable(){
					@Override
					public void run() {
					    System.out.println("accepted client.");
					    
						while(client.isConnected())
						{
					        String msg = null;
							try {
								// read client message
								msg = readNetwork(client);
						        
								// split information string
						        String[] nMessage = msg.split(",");
						        
						        // create an ant based on the information, perform all actions based on user code
						        MeineAmeise ant = new MeineAmeise(nMessage);
						        
						        // send back the commands the programming user wanted the ant to do
						        if(ant.getCommands().length() > 0)
						        	writeNetwork(client, ant.getCommands());
						        else
						        	writeNetwork(client, "dummy");
							} catch (SocketException e) {
								e.printStackTrace();
								return;
							} catch (IOException e) {
								e.printStackTrace();
							}
						}
					}
		        });
		        t.start();
        	}
        	catch(Exception e)
        	{
        		System.out.println(e);
        	}
        }
    }
	
    /**
     * Accepts a new client.
     * @param serverSocket
     * @return the new client as a connected socket
     * @throws IOException
     */
    java.net.Socket acceptNewClient(java.net.ServerSocket serverSocket) throws IOException {
    	// blocks until client connected
        java.net.Socket socket = serverSocket.accept();
        return socket;
    }
    
    /**
     * reads a string from the given connected socket (max 2048 chars).
     * @param socket
     * @return the read string
     * @throws IOException
     */
    String readNetwork(java.net.Socket socket) throws IOException {
        InputStream reader = (socket.getInputStream());
        
        byte[] buffer = new byte[2048];
        reader.read(buffer, 0, 4);
        int msgLen = bytesToInt(buffer);
        //System.out.print(msgLen + " ");
        if(msgLen < 2048)
        {
        	int charCount = reader.read(buffer, 0, msgLen); 
        	return new String(buffer, 0, charCount);
        }
        else
        	return "None";
    }
    
    /**
     * used to convert from -127..127 (java) to 0...256 (c#)
     * @param b
     * @return
     */
    public int shiftByte(byte b)
    {
    	if(b < 0)
    		return 256+b;
    	else
    		return b;
    }
    /**
     * converts a byte array into an integer
     * @param int_bytes
     * @return
     * @throws IOException
     */
    public int bytesToInt(byte[] int_bytes) throws IOException {
        return shiftByte(int_bytes[0])+shiftByte(int_bytes[1])*256+shiftByte(int_bytes[2])*256*256;
    }
    /**
     * converts an integer into a byte array
     * @param my_int
     * @return
     * @throws IOException
     */
    public byte[] intToBytes(int my_int) throws IOException {
        return ByteBuffer.allocate(4).putInt(my_int).array();
    }
    
    /**
     * writes a string into the given connected socket.
     * @param socket
     * @param message
     * @throws IOException
     */
    void writeNetwork(java.net.Socket socket, String message) throws IOException {
    	OutputStream  stream = socket.getOutputStream();
        byte[] bytes = intToBytes(message.length());
        stream.write(bytes);
        stream.write(message.getBytes("ISO-8859-1"));
    }
}
