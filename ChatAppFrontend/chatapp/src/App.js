import { HubConnectionBuilder } from "@microsoft/signalr";
import { WaitingRoom } from "./components/WaitingRoom.jsx";
import {useState, useEffect} from "react";
import {Chat} from "./components/Chat.jsx";
import {fetchMessageHistory} from "./services/message.js";
import {fetchMessagesByDate} from "./services/message.js";

import Signup from "./pages/signup/Signup.jsx";
import Login from "./pages/login/Login.jsx";
import Home from "./pages/home/Home.jsx";
import Sidebar from "./components/messages/sidebar/Sidebar.jsx";


function App() {
  const[connection, setConnection] = useState(null);
  const[conversationName, setConversationName] = useState("");
  const[messages, setMessages] = useState([]);
  const [currentUser, setCurrentUser] = useState("");

  

  const joinChat = async (nickname, conversationName) =>{
    var connection = new HubConnectionBuilder()
        .withUrl("http://localhost:5047/chat")
        .withAutomaticReconnect()
        .build();

    // connection.on("ReceiveMessage", (senderNickname, messageContent, created) =>{
    //   setMessages((messages)=>[...messages, {senderNickname, messageContent, created}]);
    // })
    connection.on("ReceiveMessage", (message) =>{
      setMessages((messages)=>[...messages, message]);
    })

    try{
      await connection.start();
      await connection.invoke("JoinChat",{nickname, conversationName});

      setConnection(connection);
      setConversationName(conversationName);
      setCurrentUser(nickname);

      fetchDataMessageHistory(conversationName);

    }catch(error){
      console.log(error);
    }
  }
const sendMessage = async(message)=>{
  connection.invoke("SendMessage", message);
  //console.log("Новое сообщение app:", message);
}


const closeConversation= async()=>{
  await connection.stop();
  setMessages([]);
  setConnection(null);
  setCurrentUser("");
}

  const fetchDataMessageHistory = async (conversationName) => {
    let messages = await fetchMessageHistory(conversationName);

    
    console.log(messages);
    setMessages(messages);
  }



const handleDeleteMessage = async (messageInfo) => {
  const updatedMessages = messages.filter((message) => message.messageId !== messageInfo.messageId);
  setMessages(updatedMessages);
};

const handleEditMessage = async (messageInfo) => {
  //const updatedMessages = messages.filter((message) => message.messageId !== messageInfo.messageId);
  setMessages(messageInfo);
};


   
	return (
    <div className="min-h-screen flex items-center justify-center">
        <div className=' flex flex-1 justify-center items-center overflow-hidden'>

        <div className='relative flex sm:h-[650px] md:h-[550px] w-full md:w-3/4 rounded-lg overflow-hidden bg-gray-200 bg-clip-padding backdrop-filter backdrop-blur-lg bg-opacity-0 '>
       <div className='absolute inset-0 border-2 border-gray-300 rounded-lg pointer-events-none'></div>
       <Sidebar/>
       
       {connection ? (
				<Chat
					messages={messages}
					conversationName={conversationName}
          sendMessage={sendMessage}
          closeConversation={closeConversation}
          currentUser={currentUser}
          deleteMessage={handleDeleteMessage} 
          editMessageApp={handleEditMessage}
				/>
			) : (
				<WaitingRoom joinChat={joinChat} />
			)} 

        </div>
        </div>
        </div>
    
	);
}

export default App;
