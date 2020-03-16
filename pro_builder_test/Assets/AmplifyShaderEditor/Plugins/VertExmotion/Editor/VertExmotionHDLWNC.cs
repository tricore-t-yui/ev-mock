using UnityEngine;
using System;
namespace AmplifyShaderEditor
{

    [Serializable]
    [NodeAttributes("VertExmotion (HD & LW RP NC)", "VertExmotion", "VertExmotion softbody\nHD & leightweight RP with normal correction")]
    public class VertExmotionHDLWNormalCorrectionASENode : VertExmotionASEParentNode
    {
        protected override void CommonInit(int uniqueId)
        {
            base.CommonInit(uniqueId);
            AddInputPort(WirePortDataType.FLOAT3, false, "World Position", -1, MasterNodePortCategory.Vertex);
            AddInputPort(WirePortDataType.COLOR, false, "Vertex Color", -1, MasterNodePortCategory.Vertex);
            AddInputPort(WirePortDataType.FLOAT3, false, "World normal", -1, MasterNodePortCategory.Vertex);
            AddInputPort(WirePortDataType.FLOAT3, false, "World tangent", -1, MasterNodePortCategory.Vertex);
            AddOutputPort(WirePortDataType.FLOAT3, "Vertex Offset");
            AddOutputPort(WirePortDataType.FLOAT3, "Vertex Normal");
            
        }

        public override string GenerateShaderForOutput(int outputId, ref MasterNodeDataCollector dataCollector, bool ignoreLocalvar)
        {            
            dataCollector.AddToDefines(UniqueId, "VERTEXMOTION_ASE_HD_LW_RP");
            UpdateVertExmotionIncludePath(ref dataCollector);

            string valueInput0 = m_inputPorts[0].GeneratePortInstructions(ref dataCollector);
            string valueInput1 = m_inputPorts[1].GeneratePortInstructions(ref dataCollector);
            string valueInput2 = m_inputPorts[2].GeneratePortInstructions(ref dataCollector);
            string valueInput3 = m_inputPorts[3].GeneratePortInstructions(ref dataCollector);

            string normalVar = "NewNormal_" + OutputId;
            string PosVar = "NewPos_" + OutputId;
            string finalCalculation = "";
            if (outputId == 0)
            {
                
                RegisterLocalVariable(0, valueInput0, ref dataCollector, PosVar);
                //finalCalculation = string.Format("VertExmotionWorldPosASE({0},{1}," + m_outputPorts[1].LocalValue(MasterNodePortCategory.Vertex) + ",{2})", valueInput0, valueInput1, valueInput3) + "/*First call*/";
                finalCalculation = string.Format("VertExmotionWorldPosASE({0},{1}," + normalVar + ",{2})", valueInput0, valueInput1, valueInput3) + "/*First call*/";
                RegisterLocalVariable(1, valueInput2, ref dataCollector, normalVar);
            }
            else
            {
                
                finalCalculation = normalVar + "/*second call*/";
            }
            return finalCalculation;
        }
    }
}