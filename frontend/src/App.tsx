import { RouteProvider } from "./routes";

function App() {
  //logout when multiple tabs are open
  window.addEventListener("storage", () => {
    if (!localStorage.getItem("token")) {
      window.location.href = "/";
    }
  });
  return (
    <>
      <RouteProvider />
    </>
  );
}

export default App;
