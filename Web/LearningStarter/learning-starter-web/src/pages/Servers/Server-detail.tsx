import { useParams } from "react-router-dom";
import { Container, Flex, Text, TextInput, Button, ScrollArea, Box } from "@mantine/core";
import { useState, useEffect } from "react";

interface Channel {
  id: number;
  name: string;
}

interface Message {
  id: number;
  sender: string;
  content: string;
}

const channels: Channel[] = [
  { id: 1, name: "General" },
  { id: 2, name: "Homework Help" },
  { id: 3, name: "Projects" },
];

export const ServerDetail = () => {
  const { serverId } = useParams();
  const [messages, setMessages] = useState<Message[]>([]);
  const [currentMessage, setCurrentMessage] = useState("");
  const [selectedChannel, setSelectedChannel] = useState<Channel | null>(channels[0]);

  const sendMessage = () => {
    if (currentMessage.trim()) {
      setMessages([
        ...messages,
        { id: Date.now(), sender: "You", content: currentMessage.trim() },
      ]);
      setCurrentMessage("");
    }
  };

  const handleChannelClick = (channel: Channel) => {
    setSelectedChannel(channel);
  };

  useEffect(() => {
    // Optionally, fetch messages from a server or database when the component mounts
  }, []);

  return (
    <Flex style={{ height: "100vh", backgroundColor: "#2f3136" }}>
      {/* Sidebar for channels */}
      <Box
        style={{
          backgroundColor: "#202225",
          width: "250px",
          padding: "10px",
          color: "#fff",
        }}
      >
        <Text fw={700} size="lg" mb="md">
          Channels
        </Text>
        {channels.map((channel) => (
          <Text
            key={channel.id}
            onClick={() => handleChannelClick(channel)}
            style={{
              padding: "10px",
              borderRadius: "5px",
              cursor: "pointer",
              backgroundColor: selectedChannel?.id === channel.id ? "#7289da" : "#2f3136",
              color: selectedChannel?.id === channel.id ? "#fff" : "#bbb",
              transition: "background-color 0.2s",
            }}
          >
            #{channel.name}
          </Text>
        ))}
      </Box>

      {/* Main Content Area */}
      <Container
        style={{
          flex: 1,
          padding: "20px",
          display: "flex",
          flexDirection: "column",
        }}
      >
        {/* Fancy Header */}
        <Flex justify="space-between" align="center" mb="lg" style={{ color: "#fff" }}>
          <Text fw={700} size="xl">
            Server: {serverId}
          </Text>
          <Button color="blue">Server Settings</Button>
        </Flex>

        {/* Chat Area */}
        <ScrollArea style={{ flex: 1, marginBottom: "10px", backgroundColor: "#36393f" }}>
          {messages.map((message) => (
            <Box
              key={message.id}
              style={{
                padding: "10px",
                borderBottom: "1px solid #444",
                color: "#fff",
              }}
            >
              <Text fw={500}>{message.sender}</Text>
              <Text>{message.content}</Text>
            </Box>
          ))}
        </ScrollArea>

        {/* Message Input */}
        <Flex align="center" style={{ marginTop: "auto" }}>
          <TextInput
            style={{ flex: 1, marginRight: "10px" }}
            placeholder="Type your message here..."
            value={currentMessage}
            onChange={(e) => setCurrentMessage(e.target.value)}
          />
          <Button onClick={sendMessage} color="green">
            Send
          </Button>
        </Flex>
      </Container>
    </Flex>
  );
};