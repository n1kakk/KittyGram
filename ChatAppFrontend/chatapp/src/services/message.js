import axios from "axios";


export const fetchMessageHistory =async(conversationId) =>{
    try {

        const url = `http://localhost:5047/Message/GetMessagesForMonth?conversationId=${conversationId}`;
        const response = await axios.get(url);

        return response.data;
    } catch (error) {
        console.error(error);
        // throw error; // Пробрасываем ошибку для обработки в вызывающем коде
    }
};


export const deleteMessageForEveryone = async (messageToDelete) => {
    try {
        await axios.delete("http://localhost:5047/Message/DeleteMessageForEveryone", {
            data: messageToDelete
        });
    } catch (e) {
        console.error(e);
    }
};


export const fetchMessagesByDate = async (conversationId, date) => {
    try {
        const formattedDate = date.toISOString().split('T')[0];
        const url = `http://localhost:5047/Message/GetMessagesByDate?conversationId=${conversationId}&date=${formattedDate}`;
        const response = await axios.get(url);

        return response.data;
    } catch (e) {
        console.error(e);

    }
};


export const updateMessage = async (messageToEdit) => {
    try {
        await axios.post("http://localhost:5047/Message/EditMessage", {
            data: messageToEdit
        });
    } catch (e) {
        console.error(e);
    }
};