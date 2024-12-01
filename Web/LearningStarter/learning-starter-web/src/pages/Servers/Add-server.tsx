import { useState } from "react";
import { Container, TextInput, Button } from "@mantine/core";
import { notifications } from "@mantine/notifications";

export const AddServer = () => {
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");

  const handleAddServer = () => {
    if (!name || !description) {
      notifications.show({
        title: "Error",
        message: "Please fill in all fields",
        color: "red",
      });
      return;
    }

    notifications.show({
      title: "Success",
      message: `Server "${name}" added successfully!`,
      color: "green",
    });

    // Add your logic to save the server here
    setName("");
    setDescription("");
  };

  return (
    <Container>
      <TextInput
        label="Server Name"
        value={name}
        onChange={(e) => setName(e.target.value)}
        placeholder="Enter server name"
        required
      />
      <TextInput
        label="Server Description"
        value={description}
        onChange={(e) => setDescription(e.target.value)}
        placeholder="Enter server description"
        required
        mt="md"
      />
      <Button onClick={handleAddServer} mt="md">
        Add Server
      </Button>
    </Container>
  );
};