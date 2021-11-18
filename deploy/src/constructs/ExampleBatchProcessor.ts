import { Construct } from "constructs";
import path from "path";
import { IParameter, StringParameter } from "aws-cdk-lib/lib/aws-ssm";
import { IGrantable } from "aws-cdk-lib/lib/aws-iam";
import { External } from "./External";
import {
  FargateBatchJob,
  FargateComputeEnvironment,
} from "@tsukiy0/aws-cdk-tools";
import { SubnetType } from "aws-cdk-lib/aws-ec2";
import { ContainerImage } from "aws-cdk-lib/aws-ecs";

export class ExampleBatchProcessor extends Construct {
  private readonly job: FargateBatchJob;
  private readonly jobQueueArnParam: IParameter;
  private readonly jobDefinitionArnParam: IParameter;

  public constructor(
    scope: Construct,
    id: string,
    props: {
      external: External;
    }
  ) {
    super(scope, id);

    const computeEnvironment = new FargateComputeEnvironment(
      this,
      "ComputeEnvironment",
      {
        vpc: props.external.vpc,
        vpcSubnets: {
          subnetType: SubnetType.PUBLIC,
        },
        spot: true,
        vcpus: 2,
      }
    );

    const job = new FargateBatchJob(this, "Job", {
      computeEnvironment,
      container: {
        image: ContainerImage.fromAsset(
          path.resolve(
            __dirname,
            "../../../Tsukiy0.Extensions.Example.Processor.Aws.Batch"
          )
        ),
        vcpus: 0.25,
        memoryLimitMiB: 512,
        assignPublicIp: true,
      },
    });
    props.external.grantTableReadWrite(job);

    const jobDefinitionArnParam = new StringParameter(
      this,
      "JobDefinitionArn",
      {
        parameterName: "/tsukiy0/extensions/batch-processor/job-definition-arn",
        stringValue: job.definition.jobDefinitionArn,
      }
    );

    const jobQueueArnParam = new StringParameter(this, "JobQueueArn", {
      parameterName: "/tsukiy0/extensions/batch-processor/job-queue-arn",
      stringValue: job.queue.jobQueueArn,
    });

    this.job = job;
    this.jobDefinitionArnParam = jobDefinitionArnParam;
    this.jobQueueArnParam = jobQueueArnParam;
  }

  public grantSubmit = (grantee: IGrantable): void => {
    this.jobDefinitionArnParam.grantRead(grantee);
    this.jobQueueArnParam.grantRead(grantee);
    this.job.grantSubmit(grantee);
  };
}
