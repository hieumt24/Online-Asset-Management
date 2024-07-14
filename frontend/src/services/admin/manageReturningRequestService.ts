import axiosInstance from "@/api/axiosInstance";
import {
  CreateReturningRequestReq,
  GetReturningRequestReq,
  UpdateReturningRequestReq,
} from "@/models";

export const getReturningRequest = (req: GetReturningRequestReq) => {
  if (req.returnState === 0) {
    delete req.returnState;
  }

  if (req.returnedDateFrom === "" || req.returnedDateTo === "") {
    delete req.returnedDateFrom;
    delete req.returnedDateTo;
  }
  
  return axiosInstance
    .post("/returnRequests/filter-return-requests", req)
    .then((res) => {
      return {
        success: true,
        message: "Requests fetched successfully!",
        data: res.data,
      };
    })
    .catch((err) => {
      return {
        success: false,
        message: err.response?.data.message || "Failed to fetch requests.",
        data: err,
      };
    });
};

export const createReturnRequest = (req: CreateReturningRequestReq) => {
  return axiosInstance
    .post("/returnRequests", req)
    .then((res) => {
      return {
        success: true,
        message: "Request created successfully!",
        data: res.data,
      };
    })
    .catch((err) => {
      return {
        success: false,
        message: err.response?.data.message || "Failed to create request.",
        data: err,
      };
    });
};

export const updateReturnRequest = (req: UpdateReturningRequestReq) => {
  return axiosInstance
    .put("/returnRequests", req)
    .then((res) => {
      return {
        success: true,
        message: "Request updated successfully!",
        data: res.data,
      };
    })
    .catch((err) => {
      return {
        success: false,
        message: err.response?.data.message || "Failed to update request.",
        data: err,
      };
    });
};

export const cancelReturnRequest = (id: string) => {
  return axiosInstance
    .delete(`/returnRequests?returnRequestId=${id}`)
    .then((res) => {
      return {
        success: true,
        message: "Request cancelled successfully!",
        data: res.data,
      };
    })
    .catch((err) => {
      return {
        success: false,
        message: err.response?.data.message || "Failed to cancel request.",
        data: err,
      };
    });
};
