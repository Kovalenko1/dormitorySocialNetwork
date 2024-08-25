import {Messenger} from "./components/Messanger/Messenger";
import {Sidebar} from "./components/Sidebar/Sidebar";


function App() {
  return (
      <main style={{display: "flex", flexDirection: "row", height: "100vh", width: "100%"}}>
          <Sidebar />
          <Messenger />
      </main>
  );
}

export default App;
