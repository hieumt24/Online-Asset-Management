import { AuthRequired } from "@/components/AuthRequired";
import { Login, NotFound } from "@/pages";
import { Forbidden } from "@/pages/Forbidden";
import { Layout } from "@/pages/Layout";
import { Route, Routes } from "react-router-dom";

export const RouteProvider: React.FC = () => {
  return (
    <Routes>
      <Route path="/auth">
        <Route path="login" element={<Login />} />
      </Route>
      <Route
        path="/*"
        element={
          <AuthRequired>
            <Layout />
          </AuthRequired>
        }
      />
      <Route path="/notfound" element={<NotFound />} />
      <Route path="/forbidden" element={<Forbidden />} />
    </Routes>
  );
};
