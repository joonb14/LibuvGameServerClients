using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyClient
{
    // 본래는 이게 필요가 없다
    // ClientSession에서 여러개의 Session이 만들어질 필요가 없기때문...
    // 하지만 여기서는 여러개의 Client를 만들어서 Test할 용도의
    // DummyClient이기 때문에 이 Session Manager를 사용하게 된다
    class SessionManager
    {
        static SessionManager _session = new SessionManager();
        public static SessionManager Instance { get { return _session; } }

        List<ServerSession> _sessions = new List<ServerSession>();
        object _lock = new object();
        Random _rand = new Random();

        public void SendForEach()
        {
            lock (_lock)
            {
                foreach(ServerSession session in _sessions)
                {
                    C_MOVE movePacket = new C_MOVE();
                    movePacket.posX = _rand.Next(-50,50);
                    movePacket.posY = 0;
                    movePacket.posZ = _rand.Next(-50, 50);
                    session.Send(movePacket.Write());
                }
            }
        }

        public ServerSession Generate()
        {
            lock (_lock)
            {
                ServerSession session = new ServerSession();
                _sessions.Add(session);
                return session;
            }
        }
    }
}
