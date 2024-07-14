import axiosInstance from "@/api/axiosInstance";
import {
  CreateAssignmentReq,
  EditAssignmentReq,
  GetAssignmentByUserAssignedReq,
  GetAssignmentReq,
  UpdateAssignmentStateReq,
} from "@/models";
export const getAllAssignmentService = (req: GetAssignmentReq) => {
  if (req.assignmentState === 0) {
    delete req.assignmentState;
  }

  if (req.dateFrom === "" || req.dateTo === "") {
    delete req.dateFrom;
    delete req.dateTo;
  }

  return axiosInstance
    .post("/assignments/filter-assignments", req)
    .then((res) => {
      return {
        success: true,
        message: "Assignments fetched successfully!",
        data: res.data,
      };
    })
    .catch((err) => {
      return {
        success: false,
        message: err.response?.data.message || "Failed to fetch assignments.",
        data: err,
      };
    });
};

export const getAssignmentByUserAssignedService = (
  req: GetAssignmentByUserAssignedReq,
) => {
  if (req.assignmentState === 0) {
    delete req.assignmentState;
  }

  if (req.dateFrom === "" || req.dateTo === "") {
    delete req.dateFrom;
    delete req.dateTo;
  }

  return axiosInstance
    .post("/assignments/filter-user-assigned", req)
    .then((res) => {
      return {
        success: true,
        message: "Assignments fetched successfully!",
        data: res.data,
      };
    })
    .catch((err) => {
      return {
        success: false,
        message: err.response?.data.message || "Failed to fetch assignments.",
        data: err,
      };
    });
};

export const getAssignmentByIdService = (id: string) => {
  return axiosInstance
    .get(`/assignments?assignmentId=${id}`)
    .then((res) => {
      return {
        success: true,
        message: "Assignment fetched successfully!",
        data: res.data,
      };
    })
    .catch((err) => {
      return {
        success: false,
        message: err.response?.data.message || "Failed to fetch assignment.",
        data: err,
      };
    });
};

export const createAssignmentService = (req: CreateAssignmentReq) => {
  return axiosInstance
    .post("/assignments", req)
    .then((res) => {
      return {
        success: true,
        message: "Assignment created successfully!",
        data: res.data,
      };
    })
    .catch((err) => {
      return {
        success: false,
        message: err.response?.data.message || "Failed to create assignment.",
        data: err,
      };
    });
};

export const updateAssignmentStateService = (req: UpdateAssignmentStateReq) => {
  return axiosInstance
    .put("/assignments", req)
    .then((res) => {
      return {
        success: true,
        message: "Assignment state updated successfully!",
        data: res.data,
      };
    })
    .catch((err) => {
      return {
        success: false,
        message:
          err.response?.data.message || "Failed to update assignment state.",
        data: err,
      };
    });
};

export const deleteAssignmentService = (id: string) => {
  return axiosInstance
    .delete(`/assignments/${id}`)
    .then((res) => {
      return {
        success: true,
        message: "Assignment deleted successfully!",
        data: res.data,
      };
    })
    .catch((err) => {
      return {
        success: false,
        message:
          err.response?.data.message || "Failed to delete assignment state.",
        data: err,
      };
    });
};

export const editAssignmentService = (id: string, req: EditAssignmentReq) => {
  return axiosInstance
    .put(`/assignments/edit-assignment/${id}`, req)
    .then((res) => {
      return {
        success: true,
        message: "Assignment edited successfully!",
        data: res.data,
      };
    })
    .catch((err) => {
      return {
        success: false,
        message: err.response?.data.message || "Failed to edit assignment.",
        data: err,
      };
    });
};
