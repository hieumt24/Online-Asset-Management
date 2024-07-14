import { AssetRes } from "@/models";
import { getAllAssetService } from "@/services/";
import { useCallback, useEffect, useState } from "react";

export const useAssets = (
  pagination: {
    pageIndex: number;
    pageSize: number;
  },
  adminLocation: number,
  search?: string,
  orderBy?: string,
  isDescending?: boolean,
  assetStateType?: Array<number>,
  categoryId?: string,
) => {
  const [assets, setAssets] = useState<AssetRes[] | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<boolean | null>(false);
  const [pageCount, setPageCount] = useState<number>(0);
  const [totalRecords, setTotalRecords] = useState<number>(0);

  // Function to fetch assets
  const fetchAssets = useCallback(async (isEdited?: string | null) => {
    setLoading(true);
    try {
      isEdited && (pagination.pageSize -= 1);
      const data = await getAllAssetService({
        pagination,
        search,
        orderBy,
        isDescending,
        adminLocation,
        assetStateType,
        categoryId,
      });

      setAssets(data.data.data || []);
      setPageCount(data.data.totalPages);
      setTotalRecords(data.data.totalRecords);
      isEdited && (pagination.pageSize += 1);
      return data.data.data || [];
    } catch (error) {
      console.error("Error in fetchAssets:", error);
      setError(true);
      return [];
    } finally {
      setLoading(false);
    }
  }, [
    pagination,
    search,
    orderBy,
    isDescending,
    adminLocation,
    assetStateType,
    categoryId,
  ]);

  useEffect(() => {
    const fetchAndUpdateAssets = async () => {
      setLoading(true);
      try {
        const isEdited = localStorage.getItem("edited");

        // Always fetch the latest data
        const currentAssets = await fetchAssets(isEdited);

        if (isEdited) {
          const orderByField = isEdited
              ? "lastModifiedOn"
              : orderBy;
          const newAssetData = await getAllAssetService({
            pagination,
            search,
            orderBy: orderByField,
            isDescending: true, // Ensure we get the most recent asset
            adminLocation,
            assetStateType,
            categoryId,
          });

          const newAsset = newAssetData.data.data[0];
          if (newAsset) {
            setAssets([
              newAsset,
              ...currentAssets.filter(
                (asset: AssetRes) => asset.id !== newAsset.id,
              ),
            ]);
          }

          localStorage.removeItem("edited");
        }
      } catch (error) {
        console.error("Error in fetchAndUpdateAssets:", error);
        setError(true);
      } finally {
        setLoading(false);
      }
    };

    fetchAndUpdateAssets();
  }, [
    pagination,
    search,
    orderBy,
    isDescending,
    adminLocation,
    assetStateType,
    categoryId,
    fetchAssets,
  ]);

  return {
    assets,
    loading,
    error,
    setAssets,
    pageCount,
    totalRecords,
    fetchAssets,
  };
};
