import { DependencyList, EffectCallback, useCallback, useEffect } from "react";

export default function useDebounce(
  effect: EffectCallback,
  dependencies: DependencyList,
  delay: number,
): void {
  const callback = useCallback(effect, dependencies);

  useEffect(() => {
    const timeout = setTimeout(callback, delay);
    return () => clearTimeout(timeout);
  }, [callback, delay]);
}
