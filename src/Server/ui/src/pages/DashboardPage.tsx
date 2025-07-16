import { useEffect, useState } from "react";
import dayjs from "dayjs";
import { LineChart } from "@mui/x-charts/LineChart";

interface Heartbeat {
  id: string;
  agentId: string;
  createdAt: string;
}

interface SeriesData {
  agentId: string;
  data: number[];
}

export default function DashboardPage() {
  const apiUrl = import.meta.env.VITE_API_URL;
    
  const [xAxisLabels, setXAxisLabels] = useState<string[]>([]);
  const [seriesData, setSeriesData] = useState<SeriesData[]>([]);

  useEffect(() => {
    fetch(`${apiUrl}/api/heartbeats`)
      .then((res) => res.json())
      .then((data: Heartbeat[]) => {
        const { labels, series } = groupByAgentAndMinute(data);
        setXAxisLabels(labels);
        setSeriesData(series);
      });
  }, []);

  const groupByAgentAndMinute = (entries: Heartbeat[]) => {
    const uniqueMinutes = new Set<string>();
    const grouped: Record<string, Record<string, number>> = {};

    entries.forEach((entry) => {
      const minute = dayjs(entry.createdAt).format("HH:mm");
      uniqueMinutes.add(minute);

      if (!grouped[entry.agentId]) {
        grouped[entry.agentId] = {};
      }

      grouped[entry.agentId][minute] = (grouped[entry.agentId][minute] || 0) + 1;
    });

    // Sort labels (time)
    const sortedMinutes = Array.from(uniqueMinutes).sort();

    const series = Object.entries(grouped).map(([agentId, data]) => {
      const counts = sortedMinutes.map((minute) => data[minute] || 0);
      return {
        agentId,
        data: counts,
      };
    });

    return {
      labels: sortedMinutes,
      series,
    };
  };

  return (
    <>
      <LineChart
        xAxis={[{ data: xAxisLabels, scaleType: "band", label: "Time" }]}
        series={seriesData.map((s) => ({
          data: s.data,
          label: `Agent ${s.agentId}`,
          area: true,
        }))}
        height={400}
      />
    </>
  );
}
