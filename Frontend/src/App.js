import Redirector from "./components/features/url-shortener/Redirector";
import { Box } from "@mui/material";

function App() {
  return (
    <Box
      sx={{
        minHeight: "100vh",         
        display: "flex",
        justifyContent: "center",    
        alignItems: "center",        
        bgcolor: "#f5f5f5",          
      }}
    >
      <Redirector />
    </Box>
  );
}

export default App;
