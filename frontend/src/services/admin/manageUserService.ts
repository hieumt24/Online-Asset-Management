import axiosInstance from "@/api/axiosInstance";
import { CreateUserReq, GetUserReq, UpdateUserReq } from "@/models";

export const createUserService = (req: CreateUserReq) => {
  return axiosInstance
    .post("/users", req)
    .then((res) => {
      return {
        success: true,
        message: "User created successfully!",
        data: res.data,
      };
    })
    .catch((err) => {
      return {
        success: false,
        message: err.response?.data.message || "Failed to create user.",
        data: err,
      };
    });
};

export const getAllUserService = (req: GetUserReq) => {
  if (req.roleType === 0) {
    delete req.roleType;
  }

  return axiosInstance
    .post("/users/filter-users", req)
    .then((res) => {
      return {
        success: true,
        message: "Users fetched successfully!",
        data: res.data,
      };
    })
    .catch((err) => {
      return {
        success: false,
        message: err.response?.data.message || "Failed to fetch users.",
        data: err,
      };
    });
};

export const getUserByIdService = (id: string) => {
  return axiosInstance
    .get(`/users/${id}`)
    .then((res) => {
      return {
        success: true,
        message: "User fetched successfully!",
        data: res.data.data,
      };
    })
    .catch((err) => {
      return {
        success: false,
        message: err.response?.data.message || "Failed to fetch user.",
        data: err,
      };
    });
};

export const getUserByStaffCodeService = (staffCode: string | undefined) => {
  return axiosInstance
    .get(`/users/staffCode/${staffCode}`)
    .then((res) => {
      return {
        success: true,
        message: "User fetched successfully!",
        data: res.data,
      };
    })
    .catch((err) => {
      return {
        success: false,
        message: err.response?.data.message || "Failed to fetch user.",
        data: err,
      };
    });
};

export const updateUserService = (req: UpdateUserReq) => {
  return axiosInstance
    .put(`/users`, req)
    .then((res) => {
      return {
        success: true,
        message: "User updated successfully!",
        data: res.data,
      };
    })
    .catch((err) => {
      return {
        success: false,
        message: err.response?.data.message || "Failed to update user.",
        data: err,
      };
    });
};

export const checkUserHasAssignmentService = (id: string) => {
  return axiosInstance.post(`/users/isValidDisableUser/${id}`).then((res) => {
    return {
      success: res.data.succeeded,
      message: res.data.message,
      data: res.data.data,
    };
  })
  .catch((err) => {
    return {
      success: false,
      message: err.response.data.message || "Failed to check if user has assignment.",
      data: err,
    };
  });
}

export const disableUserService = (id: string) => {
  return axiosInstance
    .delete(`/users/disable/${id}`)
    .then((res) => {
      return {
        success: true,
        message: "User disabled successfully!",
        data: res.data,
      };
    })
    .catch((err) => {
      return {
        success: false,
        message: err.response.data.message || "Failed to disable user.",
        data: err,
      };
    });
};

export const resetPasswordService = (id: string) => {
  return axiosInstance
    .post(`/users/resetPassword/${id}`)
    .then((res) => {
      return {
        success: true,
        message: "Password reset successfully!",
        data: res.data,
      };
    })
    .catch((err) => {
      return {
        success: false,
        message: err.response.data.message || "Failed to reset password.",
        data: err,
      };
    });
};
