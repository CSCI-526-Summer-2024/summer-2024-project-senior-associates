import json

length = [24, 24, 36]


def find_last_index(lst, value):
    try:
        return len(lst) - 1 - lst[::-1].index(value)
    except ValueError:
        return -1


kpi_trends = [[], [], []]
kpi_lengths = [[], [], []]
with open("cs526-senior-associates-default-rtdb-export.json", encoding="utf-8") as f:
    data = json.load(f)
    data = data["data"]
    for _, d in data.items():
        level = int(d["level"]) - 1
        trend = d["kpiTrend"]
        trend = trend[find_last_index(trend, 0):]
        if level == 0:
            while trend[0] < 6:
                trend = trend[1:]
        trend = trend[: length[level]]
        while len(trend) < length[level]:
            trend.append(trend[-1])
        kpi_trends[level].append(trend)
        kpi_lengths[level].append(len(trend))


for t in kpi_trends[2]:
    print(", ".join(str(i) for i in t))
