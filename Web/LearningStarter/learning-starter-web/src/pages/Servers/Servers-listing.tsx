import { Text } from "@mantine/core";
import { useState, useEffect } from "react";
import {
  Container,
  Title,
  TextInput,
  Grid,
  Card,
  Tooltip,
  Button,
  Flex,
  Avatar,
} from "@mantine/core";
import { FaHome, FaPlusCircle, FaSearch } from "react-icons/fa";
import { useNavigate } from "react-router-dom";

interface ServersGetDto {
  id: number;
  name: string;
  description: string;
  initials: string; // Add initials for the server icon
}

export const ServersListing = () => {
  const [servers, setServers] = useState<ServersGetDto[]>([
    {
      id: 1,
      name: "Econs2020 Study Group",
      description: "A place to study together",
      initials: "ES",
    },
    { id: 2, name: "Chess Gambit Club", description: "For lovers of chess", initials: "CC" },
    { id: 3, name: "Coding Homework", description: "Share coding tips and tricks", initials: "CH" },
    { id: 4, name: "Calculus Prodigy", description: "Math Homework help", initials: "CP" },
  ]);
  const [filteredServers, setFilteredServers] = useState<ServersGetDto[]>(servers);
  const [searchQuery, setSearchQuery] = useState("");

  const navigate = useNavigate();

  useEffect(() => {
    setFilteredServers(servers);
  }, [servers]);

  const handleSearch = (query: string) => {
    setSearchQuery(query);
    const filtered = servers.filter(
      (server) =>
        server.name.toLowerCase().includes(query.toLowerCase()) ||
        server.description.toLowerCase().includes(query.toLowerCase())
    );
    setFilteredServers(filtered);
  };

  const handleServerClick = (serverId: number) => {
    // Navigate to the server's page
    navigate(`/servers/${serverId}`);
  };

  return (
    <Flex style={{ height: "100vh" }}>
      {/* Sidebar */}
      <Container
        style={{
          backgroundColor: "#2f3136",
          width: "80px",
          height: "100vh",
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
          padding: "10px",
        }}
      >
        {/* Home Button */}
        <Tooltip label="Home" position="right" withArrow>
          <Button
            style={{
              backgroundColor: "#5865f2",
              borderRadius: "50%",
              height: "60px",
              width: "60px",
              marginBottom: "20px",
            }}
            onClick={() => navigate("/")}
          >
            <FaHome size={30} color="white" />
          </Button>
        </Tooltip>

        {/* Server Icons */}
        {servers.map((server) => (
          <Tooltip key={server.id} label={server.name} position="right" withArrow>
            <Avatar
              radius="xl"
              size={50}
              style={{
                backgroundColor: "#7289da",
                color: "white",
                marginBottom: "15px",
                cursor: "pointer",
              }}
              onClick={() => handleServerClick(server.id)}
            >
              {server.initials}
            </Avatar>
          </Tooltip>
        ))}

        {/* Add Server Button */}
        <Tooltip label="Add Server" position="right" withArrow>
          <Button
            style={{
              backgroundColor: "#3ba55c",
              borderRadius: "50%",
              height: "60px",
              width: "60px",
              marginTop: "auto",
            }}
            onClick={() => navigate("/Add-server")}
          >
            <FaPlusCircle size={30} color="white" />
          </Button>
        </Tooltip>
      </Container>

      {/* Main Content */}
      <Container style={{ flex: 1, padding: "20px" }}>
        <TextInput
          leftSection={<FaSearch />}
          placeholder="Search for a server"
          value={searchQuery}
          onChange={(e) => handleSearch(e.target.value)}
          style={{ width: "100%", maxWidth: "400px", marginBottom: "20px" }}
        />
        <Title order={2} style={{ marginBottom: "20px", textAlign: "center" }}>
          {filteredServers.length === 0 ? "No servers found" : "Servers"}
        </Title>
        <Grid>
          {filteredServers.map((server) => (
            <Grid.Col span={4} key={server.id}>
              <Card shadow="sm" padding="lg">
                <Text style={{ fontWeight: 500 }}>{server.name}</Text>
                <Text style={{ fontSize: "small", color: "dimmed" }}>{server.description}</Text>
              </Card>
            </Grid.Col>
          ))}
        </Grid>
      </Container>
    </Flex>
  );
};
