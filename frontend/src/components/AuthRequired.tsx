import { useAuth } from "@/hooks";
import { PropsWithChildren } from "react";
import { Navigate } from "react-router-dom";

export const AuthRequired = ({children}: PropsWithChildren) => {
    const {isAuthenticated} = useAuth();

    return (
        isAuthenticated ? children : <Navigate to="/auth/login"/>
    );
};