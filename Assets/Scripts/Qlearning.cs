using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumSharp;
using UnityEngine;
namespace ML {

    public class Qlearning : MonoBehaviour {


        NDArray qTable = null;
        NDArray rewardsTable = null;
        [SerializeField] Environment environment;
        [SerializeField] int totalEpisodes = 10000  ;//this refers to the number of episodes we want or agent to play
        [SerializeField] int stepPerEpisode = 50  ;//refers to the Maxnumber of steps at each episode , if exceeded the episode will end immeditely
        [SerializeField] double learningRate  = 0.1 ;// α : rate of updating Q value
        [SerializeField] double discountRate  = 0.99 ;// 𝛾 : rate of decay of rewards
        [SerializeField] double explorationRate = 1 ;//ε : prob. of our agent exploring the environment instead of exploiting it
        [SerializeField] double maxExplorationRate = 1;
        [SerializeField] double minExplorationRate = 0.01;
        [SerializeField] double explorationDecay = 0.009;
        List<float> rewards_all_episodes = new List<float>();// we will use this to visulatize the rewards for each episode

        // Start is called before the first frame update
        void Start() {
            //Environment.Initialize
            environment = gameObject.GetComponent<CrawlerEnvironment>();
            if(environment !=null)
            Debug.Log(environment);
            Qlearn();

        }

        /// <summary>
        ///  quick reminder of what we want to do in the algorithm
        ///  at the beggining of each episdoe we need to reset the state to inital
        ///  now for each step in the episdoe we need to randomly select wether to explore or exploit according to a random value r realtive to the ε value (r<ε -> explore) (r>ε -> exploit)
        ///  take an acction with explor-exploit in-mind
        ///  updating our Q-table values
        ///  then set our state to the new state and add the reward to the total rewards of the episode
        ///  finally we would update ε using exploration decay rate
        /// </summary>
        ///
        private async void Qlearn()
        {
            if(environment == null)
             await Task.Yield();

            var actionSpace =  environment.actionsNumber;
            var stateSpace =  environment.statesNumber;

            qTable = np.zeros(stateSpace,actionSpace);
            rewardsTable = np.zeros(stateSpace,actionSpace);
            Print_Qtable();

            Debug.Log("Action Space is "+ actionSpace);
            Debug.Log("State Space is"+ stateSpace);
            ML.Environment.States currentState = null;

            for(int episode =0;episode<totalEpisodes;episode++)
            {
                currentState  = environment.IntializeEnvironment();
                var done = false;
                float totalEpisodeReward = 0;

                //for each step
                for(int step = 0 ;step < stepPerEpisode; step++)
                {
                    //exploration vs exploitation
                    var random = np.random.uniform(0,1);
                    ML.Environment.Actions action ;
                    if(random<=explorationRate)
                    {
                        //Explore
                        action = new ML.Environment.Actions(np.random.randint(0,actionSpace-1));
                    }else
                    {
                        //exploit
                        action = new ML.Environment.Actions(np.argmax(qTable[currentState.value,":"]));
                    }
                    //take action in environment

                    float reward = -0.5f; ML.Environment.States newState = null;
                    done = true;

                    var statereward = await environment.TakeAction(action);
                    newState = statereward.GetState();
                    reward = statereward.GetReward();
                    done = environment.IsTerminalState();

                    Debug.Log("Old state "+currentState.value+" Action Taken "+ action.value+", New State : "+ newState.value+" Reward : " +reward);

                    rewardsTable[currentState.value,action.value] = reward;
                    qTable[currentState.value,action.value] = (1-learningRate) * qTable[currentState.value,action.value] + learningRate * (reward + discountRate * (np.max(qTable[newState.value,":"])));
                    totalEpisodeReward += reward;
                    currentState = newState;
                    if(done)
                    {
                        Debug.Log("Last Reward : " + reward+ "Last State : " + currentState);
                        break;
                    }

                    //updating exploration rate
                    rewards_all_episodes.Append(totalEpisodeReward);
                    await Task.Yield();
                }
                explorationRate = minExplorationRate + (maxExplorationRate - minExplorationRate) * np.exp(-explorationDecay * episode);
                Print_rewardTable();
                await Task.Yield();
            }

            Print_Qtable();
            await Task.Yield();
        }

        private void Print_Qtable() {

            StringBuilder sb = new StringBuilder();
            for(int i=0; i< qTable.shape[0] ; i++)
            {
                for(int j=0; j<qTable.shape[1]; j++)
                {
                    sb.Append((byte)qTable[i,j]);
                    sb.Append(' ');
                }
                sb.AppendLine();
            }
            Debug.Log(sb.ToString());
        }


        private void Print_rewardTable() {

            StringBuilder sb = new StringBuilder();
            for(int i=0; i< rewardsTable.shape[0] ; i++)
            {
                for(int j=0; j<rewardsTable.shape[1]; j++)
                {
                    sb.Append((byte)rewardsTable[i,j]);
                    sb.Append(' ');
                }
                sb.AppendLine();
            }
            Debug.Log(sb.ToString());
        }
    }
}