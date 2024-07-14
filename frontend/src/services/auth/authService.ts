import axiosInstance from "@/api/axiosInstance";
import { FirstTimeLoginReq, LoginReq } from "@/models";

export const loginService = (req: LoginReq) => {
  return axiosInstance
    .post("/auth/login", req)
    .then((res) => {
      localStorage.setItem("token", res.data.data.token);
      return {
        success: true,
        message: "Login successfully!",
        data: res.data.data,
      };
    })
    .catch((err) => {
      return {
        success: false,
        message: "Failed to login.",
        data: err.response.data,
      };
    });
};

export const firstTimeService = (req: FirstTimeLoginReq) => {
  return axiosInstance
    .post("/auth/change-password", req)
    .then((res) => {
      return {
        success: true,
        message: "Change password successfully!",
        data: res.data,
      };
    })
    .catch((err) => {
      console.log(err.response);
      return {
        success: false,
        message: err.response.data.message,
        data: err,
      };
    });
};
