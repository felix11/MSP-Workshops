// Learn more about F# at http://fsharp.net

namespace Useful

module Network =
    open System
    open System.Net
    open System.Net.Sockets
    open System.Text
    open System.IO
    open System.Threading

    // creates a server
    let CreateServer (hostname:string) (port:int) =
        let s = new TcpListener(IPAddress.Parse(hostname), port)
        do s.Start()
        s

    // creates a client
    let CreateClient (hostname:string) (port:int) =
        new TcpClient(hostname, port)

    // create a client on localhost with a standard port
    let CreateLocalClient(port) =
        CreateClient "127.0.0.1" port

    // create a server on localhost with a standard port
    let CreateLocalServer(port) =
        CreateServer "127.0.0.1" port

    // sends a string as well as its length
    let send (client:TcpClient, msg:string) =
        //printfn "sent %s" msg
        let stream = client.GetStream()
        let msgBuffer = Encoding.Unicode.GetBytes(msg)
        let buffer = BitConverter.GetBytes(msgBuffer.Length)
        do stream.Write(buffer, 0, 4)
        do stream.Flush()
        do stream.Write(msgBuffer, 0, msgBuffer.Length)
        do stream.Flush()

    // receives a string as well as its length
    let receive (client:TcpClient) =
        try
            let stream = client.GetStream()
            let buffer = Array.zeroCreate 4
            let readBytes = stream.Read(buffer , 0 , 4)
            if readBytes = 0 then
                failwith "Client disconnected"
            else
                let msgSize = BitConverter.ToInt32(buffer , 0)
                let buffer = Array.zeroCreate msgSize
                let readBytes = stream.Read(buffer, 0, msgSize)
                if readBytes = 0 then failwith "Client disconnected"
                else
                    let msg = System.Text.Encoding.Unicode.GetString(buffer)
                   // printfn "received %s" msg
                    msg
        with
            | :? System.IO.IOException -> failwith "Client disconnected"

    let handleRequests (server:TcpListener) callback =
        let worker = new Thread(new ThreadStart(fun() ->
                                while Thread.CurrentThread.IsAlive do
                                    try callback(receive (server.AcceptTcpClient()))
                                    with e -> ()))
        do worker.Start()
        worker