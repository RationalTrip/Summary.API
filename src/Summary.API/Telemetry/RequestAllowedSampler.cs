using OpenTelemetry.Trace;
using System.Diagnostics;

namespace Summary.API.Telemetry;

public class RequestAllowedSampler : Sampler
{
    private readonly Sampler _ratioSampler;

    public RequestAllowedSampler(double samplingRatio)
    {
        _ratioSampler = new TraceIdRatioBasedSampler(samplingRatio);
    }

    public override SamplingResult ShouldSample(in SamplingParameters samplingParameters)
    {
        if (samplingParameters.Kind == ActivityKind.Server)
            return new SamplingResult(SamplingDecision.RecordAndSample);

        return _ratioSampler.ShouldSample(samplingParameters);
    }
}
