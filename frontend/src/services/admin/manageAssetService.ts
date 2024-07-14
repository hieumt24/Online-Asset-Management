import axiosInstance from "@/api/axiosInstance";
import {
  CreateAssetReq,
  CreateCategoryReq,
  GetAssetReq,
  GetAssignmentByAssetReq,
  UpdateAssetReq,
} from "@/models";

export const getAllAssetService = (req: GetAssetReq) => {
  if (req.categoryId === "all") {
    delete req.categoryId;
  }
  if (req.assetStateType?.length == 0) {
    delete req.assetStateType;
  }
  return axiosInstance
    .post("/assets/filter-assets", req)
    .then((res) => {
      return {
        success: true,
        message: "Assets fetched successfully!",
        data: res.data,
      };
    })
    .catch((err) => {
      return {
        success: false,
        message: err.response?.data.message || "Failed to fetch assets.",
        data: err,
      };
    });
};

export const getAssetByIdService = (id: string) => {
  return axiosInstance
    .get(`/assets/${id}`)
    .then((res) => {
      return {
        success: true,
        message: "Asset fetched successfully!",
        data: res.data.data,
      };
    })
    .catch((err) => {
      return {
        success: false,
        message: err.response?.data.message || "Failed to fetch asset.",
        data: err,
      };
    });
};

export const deleteAssetByIdService = (id: string) => {
  return axiosInstance
    .delete(`/assets/${id}`)
    .then((res) => {
      return {
        success: true,
        message: "Asset deleted successfully!",
        data: res.data,
      };
    })
    .catch((err) => {
      return {
        success: false,
        message: err.response?.data.message || "Failed to delete asset.",
        data: err,
      };
    });
};

export const getAllCategoryService = () => {
  return axiosInstance
    .get("/categories")
    .then((res) => {
      return {
        success: true,
        message: "Categories fetched successfully!",
        data: res.data,
      };
    })
    .catch((err) => {
      return {
        success: false,
        message: err.response?.data.message || "Failed to fetch categories.",
        data: err,
      };
    });
};

export const createCategoryService = (req: CreateCategoryReq) => {
  return axiosInstance
    .post("/categories", req)
    .then((res) => {
      return {
        success: true,
        message: "Category created successfully!",
        data: res.data,
      };
    })
    .catch((err) => {
      console.log(err.response?.data);
      return {
        success: false,
        message: err.response?.data.message || "Failed to create category.",
        data: err.response,
      };
    });
};

export const createAssetService = (req: CreateAssetReq) => {
  return axiosInstance
    .post("/assets", req)
    .then((res) => {
      return {
        success: true,
        message: "Asset created successfully!",
        data: res.data,
      };
    })
    .catch((err) => {
      console.log(err.response?.data);
      return {
        success: false,
        message: err.response?.data.message || "Failed to create asset.",
        data: err.response,
      };
    });
};

export const updateAssetService = (id: string, req: UpdateAssetReq) => {
  return axiosInstance
    .put(`/assets?id=${id}`, req)
    .then((res) => {
      return {
        success: true,
        message: "Asset updated successfully!",
        data: res.data,
      };
    })
    .catch((err) => {
      console.log(err.response?.data);
      return {
        success: false,
        message: err.response?.data.message || "Failed to update asset.",
        data: err.response,
      };
    });
};

export const getAssetByAssetCodeService = (staffCode: string) => {
  return axiosInstance
    .get(`/assets/assetCode/${staffCode}`)
    .then((res) => {
      return {
        success: true,
        message: "Asset fetched successfully!",
        data: res.data.data,
      };
    })
    .catch((err) => {
      console.log(err.response?.data);
      return {
        success: false,
        message: err.response?.data.message || "Failed to fetch assets.",
        data: err.response,
      };
    });
};

export const getAssignmentByAssetService = (req: GetAssignmentByAssetReq) => {
  return axiosInstance
    .post(`/assignments/filter-asset-assigned-history/`, req)
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
        data: err.response,
      };
    });
};

export const checkAssetHasAssignmentService = (id: string) => {
  return axiosInstance.post(`/assets/isValidDeleteAsset/${id}`).then((res) => {
    return {
      success: res.data.succeeded,
      message: res.data.message,
      data: res.data.data
    };
  })
  .catch((err) => {
    return {
      success: false,
      message: err.response.data.message || "Failed to check if asset has assignment.",
      data: err
    };
  });
}