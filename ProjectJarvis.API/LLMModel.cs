namespace ProjectJarvis;

// I am wanting to use chatgbt 4o as the model for jarvis
// For now this is hard coded but later on I can have the post requests attach model data as well.
public enum LLMModel { // Large l
    Llama3_2 = 0,
    DeepSeekR1 = 1, 
    Lumimaid0_2 = 2
}